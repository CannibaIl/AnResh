using Anresh.Application.Services.Auth.Interfaces;
using Anresh.Application.Services.User.Contracts;
using Anresh.Application.Services.User.Interfaces;
using Anresh.Domain.DTO;
using Anresh.Domain.Repositories;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Anresh.Application.Services.User.Implementations
{
    public sealed class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly Options _options;
        private readonly IAuthService _authService;

        public UserService(
            IUserRepository userRepository,
            IOptions<Options> options,
            IAuthService authService
            )
        {
            _userRepository = userRepository;
            _options = options.Value;
            _authService = authService;
        }

        //public byte[] GenerateSaltedHash(string password)
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

        //public bool VerifyPassword(string password, byte[] hashPass)
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

        public async Task<string> CreateAsync(Create request)
        {
            var currentUserId = _authService.GetCurrentUserId();
            if (currentUserId is null)
            {
                throw new Exception($"Current user no found");
            }

            if (await _userRepository.IsAdmin((int)currentUserId) is false)
            {
                throw new Exception($"You don't have rights");
            }

            if (await _userRepository.FindByEmailAsync(request.Email) is not null)
            {
                throw new Exception($"Such a user already exists");
            }

            var user = new Domain.User()
            {
                EmployeeId = request.EmployeeId,
                Email = request.Email,
                Role = request.Role,
                SaltPassword = null,
                HashPassword = null
            };
            user.Id = await _userRepository.SaveAsync(user);

            var token = _authService.CreateToken(user.Id);
            //var message = $"<a href=\"{_options.FrontUri}/confirm/{token}\">confirm email</a>";
            //SendEmail(request.Email, "Сomplete registration", message);

            return $"{_options.FrontUri}/confirm/{token}";
        }

        public async Task EmailConfirmAsync(string token, string password)
        {
            var objectId = new JwtSecurityTokenHandler()
                                            .ReadJwtToken(token)
                                            .Payload
                                            .GetValueOrDefault(ClaimTypes.NameIdentifier);
            if (objectId is null)
            {
                throw new KeyNotFoundException($"bad token");
            }

            if (int.TryParse((string)objectId, out int id) is false)
            {
                throw new KeyNotFoundException($"bad token");
            }

            var user = await _userRepository.FindByIdAsync(id);
            if (user is null)
            {
                throw new KeyNotFoundException($"User not found");
            }

            var hashSalt = GenerateSaltedHash(password);

            user.HasEmailConfirm = true;
            user.HashPassword = hashSalt.Hash;
            user.SaltPassword = hashSalt.Salt;

            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(int id)
        {
            var currentUserId = _authService.GetCurrentUserId();
            if (currentUserId is null)
            {
                throw new Exception($"Bad token");
            }

            if (await _userRepository.IsAdmin((int)currentUserId) is false)
            {
                throw new Exception($"You don't have rights");
            }

            if (await _userRepository.IsExistsAsync(id) is false)
            {
                throw new Exception($"The user being deleted was not found");
            }

            await _userRepository.DeleteAsync(id);
        }

        public async Task<Authenticate.Response> AuthenticateAsync(Authenticate.Request request)
        {
            var user = await _userRepository.FindByEmailAsync(request.Email);
            if (user is null)
            {
                throw new KeyNotFoundException($"UserName or password entered incorrectly");
            }

            var verifyPassword = VerifyPassword(request.Password, user.HashPassword, user.SaltPassword);
            if (verifyPassword is false)
            {
                throw new KeyNotFoundException($"UserName or password entered incorrectly");
            }

            if (user.HasEmailConfirm is false)
            {
                throw new KeyNotFoundException($"Confirm email");
            }

            var token = _authService.CreateToken(user.Id);
            return new Authenticate.Response()
            {
                Token = token
            };
        }

        public async Task<UserDto> GetCurrentUserAsync()
        {
            return await _authService.GetCurrentUserAsync();
        }

        public async Task<UserDto> GetByEmployeeIdAsync(int employeeId)
        {
            return await _userRepository.FindByEmployeeIdAsync(employeeId);
        }

        public async Task ChangeRoleAsync(int id, string role)
        {
            var currentUserId = _authService.GetCurrentUserId();
            if (currentUserId is null)
            {
                throw new Exception($"Bad token");
            }
            
            if (await _userRepository.IsAdmin((int)currentUserId) is false)
            {
                throw new Exception($"You don't have rights");
            }
            
            if (await _userRepository.IsExistsAsync(id) is false)
            {
                throw new Exception($"User with id: { id } not found");
            }
            await _userRepository.ChangeRoleAsync(id, role);
        }

        public async Task<string> SendRestorePasswordEmailAsync(string email)
        {
            var user = await _userRepository.FindByEmailAsync(email);
            if (user is null)
            {
                throw new Exception($"User with email: { email } not found");
            }

            var token = _authService.CreateToken(user.Id);
            var message = $"<a href=\"{_options.FrontUri}/restore/{token}\">recover password</a>";
            SendEmail(email, "recover password", message);

            return $"{_options.FrontUri}/restore/{token}";
        }

        public async Task<RestorePassword.Response> RestorePasswordAsync(RestorePassword.Request request)
        {
            var id = _authService.GetUserIdByToken(request.Token);
            if (id is null)
            {
                throw new Exception($"Bad token");
            }
            
            var user = await _userRepository.FindByIdAsync((int)id);
            if (user is null)
            {
                throw new Exception($"User not found");
            }

            var saltedHash = GenerateSaltedHash(request.Password);
            await _userRepository.RestorePasswordAsync(id: (int)id, 
                                                       hash: saltedHash.Hash, 
                                                       salt: saltedHash.Salt);

            return new RestorePassword.Response()
            {
                Email = user.Email
            };
        }


    }
}
