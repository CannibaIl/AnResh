using Anresh.Application.Services.Auth.Interfaces;
using Anresh.Domain.DTO;
using Anresh.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Anresh.Application.Services.Auth.Implementations
{
    public sealed class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly Options _options;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(
            IUserRepository userRepository,
            IOptions<Options> options,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _userRepository = userRepository;
            _options = options.Value;
            _httpContextAccessor = httpContextAccessor;
        }

        public string CreateToken(int id)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _options.ApiUri,
                audience: _options.ApiUri,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Token.Key)),
                    SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public int? GetCurrentUserId()
        {
            var claim = _httpContextAccessor.HttpContext?
                        .User.FindFirst(ClaimTypes.NameIdentifier);

            return int.TryParse(claim.Value, out int id) ? id : null;
        }

        public async Task<UserDto> GetCurrentUserAsync()
        {
            var id = GetCurrentUserId();
            if (id is null)
            {
                throw new Exception("User not found");
            }
            return await _userRepository.FindByIdWithEmployeeDataAsync((int)id);
        }

        public int? GetUserIdByToken(string token)
        {
            var objectId = new JwtSecurityTokenHandler()
                                            .ReadJwtToken(token)
                                            .Payload
                                            .GetValueOrDefault(ClaimTypes.NameIdentifier);

            return objectId is null ? null 
                   : int.TryParse((string)objectId, out int id) is true ? id : null;
        }
    }
}
