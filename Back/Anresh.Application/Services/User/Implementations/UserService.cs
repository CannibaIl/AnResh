using Anresh.Application.Services.Employee.Interfaces;
using Anresh.Application.Services.User.Contracts;
using Anresh.Application.Services.User.Interfaces;
using Anresh.Domain.DTO;
using Anresh.Domain.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Anresh.Application.Services.User.Implementations
{
    public sealed class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeService _employeeService;
        private readonly Options _options;

        public UserService(
            IUserRepository userRepository,
            IEmployeeRepository employeeRepository,
            IEmployeeService employeeService,
            IOptions<Options> options
            )
        {
            _userRepository = userRepository;
            _employeeRepository = employeeRepository;
            _employeeService = employeeService;
            _options = options.Value;
        }
        //private static SaltedHash GenerateSaltedHash(string password)
        //{
        //    var byteSize = new Random().Next(20, 50);
        //    var saltBytes = new byte[byteSize];
        //    var provider = new RNGCryptoServiceProvider();
        //    provider.GetNonZeroBytes(saltBytes);
        //    var saltPassword = Convert.ToBase64String(saltBytes);

        //    var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10000);
        //    var hashPassword = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));

        //    return new SaltedHash()
        //    {
        //        Hash = hashPassword,
        //        Salt = saltPassword
        //    };
        //}

        //private static bool VerifyPassword(string requestPassword, string storedHash, string storedSalt)
        //{
        //    var saltBytes = Convert.FromBase64String(storedSalt);
        //    var rfc2898DeriveBytes = new Rfc2898DeriveBytes(requestPassword, saltBytes, 10000);
        //    return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256)) == storedHash;
        //}

        private void SendEmail(string mail, string subject, string body)
        {
            var settings = _options.MailSettings;
            var mailMessage = new MailMessage(new MailAddress(settings.Email, settings.Name), new MailAddress(mail))
            {
                Subject = subject,
                IsBodyHtml = true,
                Body = body,
            };

            var smtpClient = new SmtpClient(settings.Smtp, settings.Port)
            {
                Credentials = new NetworkCredential(settings.Email, settings.Password),
                EnableSsl = true,
            };

            smtpClient.Send(mailMessage);
        }

        //public byte[] GetHashPassword(string password)
        //{
        //    var salt = new byte[20];
        //    new RNGCryptoServiceProvider().GetBytes(salt);

        //    var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
        //    var hash = pbkdf2.GetBytes(20);

        //    var hashBytes = new byte[40];
        //    Array.Copy(salt, 0, hashBytes, 0, 20);
        //    Array.Copy(hash, 0, hashBytes, 20, 20);

        //    return hashBytes;
        //}

        //public bool IsValidPassword(string password, byte[] hashPass)
        //{
        //    var salt = new byte[20];
        //    Array.Copy(hashPass, 0, salt, 0, 20);

        //    var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
        //    var hash = pbkdf2.GetBytes(20);

        //    for (int i = 0; i < 20; i++)
        //    {
        //        if (hashPass[i + 20] != hash[i])
        //        {
        //            return false;
        //        }
        //    }

        //    return true;
        //}

        private static SaltedHash GenerateSaltedHash(string password)
        {
            var byteSize = new Random().Next(20, 50);
            var saltPassword = new byte[byteSize];
            var provider = new RNGCryptoServiceProvider();
            provider.GetNonZeroBytes(saltPassword);

            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltPassword, 10000);
            var hashPassword = rfc2898DeriveBytes.GetBytes(256);

            return new SaltedHash()
            {
                Hash = hashPassword,
                Salt = saltPassword
            };
        }

        private static bool VerifyPassword(string requestPassword, byte[] storedHash, byte[] storedSalt)
        {
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(requestPassword, storedSalt, 10000);
            var request = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));
            var stored = Convert.ToBase64String(storedHash);
            return request == stored;
        }

        private string CreateToken(Domain.User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.EmployeeId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken
            (
                issuer: _options.ApiUri,
                audience: _options.ApiUri,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Token.Key)),
                    SecurityAlgorithms.HmacSha256
                )
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<Create.Response> CreateAsync(Create.Request request)
        {
            var currentUser = _userRepository.GetCurrentUser();
            if (currentUser.Role != Domain.RoleConstants.Admin)
            {
                throw new Exception($"You don't have rights");
            }

            if (await _userRepository.FindByEmailAsync(request.Email) != null)
            {
                throw new Exception($"Such a user already exists");
            }

            var saltedHash = GenerateSaltedHash(request.Password);

            var user = new Domain.User()
            {
                EmployeeId = request.EmployeeId,
                Email = request.Email,
                Role = request.Role,
                SaltPassword = saltedHash.Salt,
                HashPassword = saltedHash.Hash
            };
            user.Id = await _userRepository.SaveAsync(user);

            //var token = CreateToken(user);
            //var message = $"<a href=\"{_options.FrontUri}/confirm/{token}\">confirm email</a>";
            //SendEmail(request.Email, "Register confirm", message);

            return new Create.Response()
            {
                Id = user.Id,
                EmployeeId = user.EmployeeId,
                Email = user.Email,
                Role = user.Role,
            };
        }

        public async Task DeleteAsync(int id)
        {
            var currentUser = _userRepository.GetCurrentUser();
            if (currentUser.Role == Domain.RoleConstants.Admin)
            {
                if (await _userRepository.IsExistsAsync(id) == false)
                {
                    throw new Exception($"User not found");
                }
                await _userRepository.DeleteAsync(id);
            }
            else
            {
                throw new Exception($"You don't have rights");
            }
        }

        public async Task<Register.Response> RegisterAsync(Register.Request request)
        {
            if (await _userRepository.FindByEmailAsync(request.Email) != null)
            {
                throw new Exception($"Such a user already exists");
            }
            if (await _employeeRepository.FindByIdAsync(request.EmployeeId) == null)
            {
                throw new Exception($"Such a user already exists");
            }

            var saltedHash = GenerateSaltedHash(request.Password);
            var user = new Domain.User()
            {
                EmployeeId = request.EmployeeId,
                Email = request.Email,
                HashPassword = saltedHash.Hash,
                SaltPassword = saltedHash.Salt,
                Role = Domain.RoleConstants.User,
                HasEmailConfirm = false
            };
            user.Id = await _userRepository.SaveAsync(user);

            var token = CreateToken(user);
            //var message = $"<a href=\"{_options.FrontUri}/confirm/{token}\">confirm email</a>";
            //SendEmail(request.Email, "Register confirm", message);

            return new Register.Response()
            {
                Id = user.Id,
                EmployeeId = user.EmployeeId,
                Email = user.Email,
                Role = user.Role,
                Token = token
            };
        }

        public async Task<bool> EmailConfirmAsync(string token)
        {
            var objectId = new JwtSecurityTokenHandler()
                                            .ReadJwtToken(token)
                                            .Payload
                                            .GetValueOrDefault(ClaimTypes.NameIdentifier);
            if (objectId == null)
            {
                throw new KeyNotFoundException($"bad token");
            }

            bool hasId = int.TryParse((string)objectId, out int id);
            if (hasId == false)
            {
                throw new KeyNotFoundException($"bad token");
            }

            var user = await _userRepository.FindByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User not found");
            }

            bool result;
            if (user.HasEmailConfirm)
            {
                throw new Exception("Email confirmed");
            }

            else
            {
                user.HasEmailConfirm = true;
                await _userRepository.UpdateAsync(user);
                result = true;
            }
            return result;
        }

        public async Task<Authenticate.Response> AuthenticateAsync(Authenticate.Request request)
        {
            var user = await _userRepository.FindByEmailAsync(request.UserName);
            if (user == null)
            {
                throw new KeyNotFoundException($"UserName or password entered incorrectly");
            }

            var verifyPassword = VerifyPassword(request.Password, user.HashPassword, user.SaltPassword);
            if (verifyPassword == false)
            {
                throw new KeyNotFoundException($"UserName or password entered incorrectly");
            }

            if (user.HasEmailConfirm == false)
            {
                throw new KeyNotFoundException($"Confirm email");
            }

            var token = CreateToken(user);
            return new Authenticate.Response()
            {
                Token = token
            };
        }

        public async Task<UserDto> GetCurrentUserAsync()
        {
            var currentUser = _userRepository.GetCurrentUser();

            if (currentUser.EmployeeId == default)
            {
                throw new Exception("User not found");
            }

            var employee = await _employeeService.GetByIdAsync(currentUser.EmployeeId);

            return new UserDto()
            {
                Id = currentUser.Id,
                Email = currentUser.Email,
                Role = currentUser.Role,
                EmployeeId = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                MiddleName = employee.MiddleName,
                DepartmentId = employee.DepartmentId,
                Salary = employee.Salary,
            };
        }

        public async Task<UserDto> GetByEmployeeIdAsync(int employeeId)
        {
            return await _userRepository.GetByEmployeeIdAsync(employeeId);
        }

        public async Task ChangeRoleAsync(int id, string role)
        {
            var currentUser = _userRepository.GetCurrentUser();
            if (currentUser.Role == Domain.RoleConstants.Admin)
            {
                if (await _userRepository.IsExistsAsync(id) == false)
                {
                    throw new Exception($"User with id: { id } not found");
                }
                await _userRepository.ChangeRoleAsync(id, role);
            }
            else
            {
                throw new Exception($"You don't have rights");
            }
        }

        public async Task<string> SendRestorePasswordEmailAsync(string email)
        {
            var user = await _userRepository.FindByEmailAsync(email);
            if (user == null)
            {
                throw new Exception($"User with email: { email } not found");
            }

            var token = CreateToken(user);
            //var message = $"<a href=\"{_options.FrontUri}/restore/{token}\">recover password</a>";
            //SendEmail(email, "recover password", message);

            return $"{_options.FrontUri}/restore/{token}";
        }

        public async Task<RestorePassword.Response> RestorePasswordAsync(RestorePassword.Request request)
        {
            var id = _userRepository.GetUserIdByToken(request.Token);
            if (id == null)
            {
                throw new Exception($"Bad token");
            }
            var user = await _userRepository.FindByIdAsync((int)id);
            if (user == null)
            {
                throw new Exception($"User not found");
            }

            var saltedHash = GenerateSaltedHash(request.Password);
            await _userRepository.RestorePasswordAsync((int)id, saltedHash.Hash, saltedHash.Salt);

            return new RestorePassword.Response()
            {
                Email = user.Email
            };
        }


    }
}
