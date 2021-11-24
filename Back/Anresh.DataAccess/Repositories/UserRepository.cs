using Anresh.Domain;
using Anresh.Domain.Repositories;
using Dapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Anresh.DataAccess.Repositories
{
    public sealed class UserRepository : GenericRepository<User, int>, IUserRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserRepository(IDbConnection db, IHttpContextAccessor httpContextAccessor) : base(db)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            var sql = $"SELECT * FROM {TableName} WHERE Email = @email";
            return await DbConnection.QuerySingleOrDefaultAsync<User>(sql, new { email });
        }
        public User GetCurrentUser()
        {
            var claimsPrincipal = _httpContextAccessor.HttpContext?.User;
            if (claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                var isId = int.TryParse((string)claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value, out int id);
                var isEmployeeId = int.TryParse((string)claimsPrincipal.FindFirst(ClaimTypes.Name).Value, out int employeeId);

                return new User()
                {
                    Id = isId ? id : default,
                    EmployeeId = isEmployeeId ? employeeId : default,
                    Email = claimsPrincipal.FindFirst(ClaimTypes.Email).Value,
                    Role = claimsPrincipal.FindFirst(ClaimTypes.Role).Value,
                };
            }
            return null;
        }

        public int? GetUserIdByToken(string token)
        {
            var objectId = new JwtSecurityTokenHandler()
                                            .ReadJwtToken(token)
                                            .Payload
                                            .GetValueOrDefault(ClaimTypes.NameIdentifier);
            if (objectId == null)
            {
                return null;
            }

            bool hasId = int.TryParse((string)objectId, out int id);
            if (hasId == false)
            {
                return null;
            }

            return id;
        }

        public async Task<Domain.DTO.UserDto> GetByEmployeeIdAsync(int employeeId)
        {
            var sql = @"SELECT u.*, e.* 
                         FROM Users u LEFT JOIN Employees e ON u.EmployeeId = e.Id 
                         WHERE u.EmployeeId = @employeeId";

            return await DbConnection.QuerySingleOrDefaultAsync<Domain.DTO.UserDto>(sql, new { employeeId });
        }
        public async Task ChangeRoleAsync(int id, string role)
        {
            var sql = @"UPDATE Users SET Role = @role WHERE Id = @id";
            await DbConnection.ExecuteAsync(sql, new { id , role});
        }

        public async Task RestorePasswordAsync(int id, byte[] hash, byte[] salt)
        {
            var sql = @"UPDATE Users SET HashPassword = @hash, SaltPassword = @salt WHERE Id = @id";
            await DbConnection.ExecuteAsync(sql, new { id, hash, salt });
        }
    }
}