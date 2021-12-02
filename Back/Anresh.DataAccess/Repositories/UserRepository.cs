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
        public UserRepository(IDbConnection db) : base(db)
        {
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            var sql = $"SELECT * FROM {TableName} WHERE Email = @email";
            return await DbConnection.QuerySingleOrDefaultAsync<User>(sql, new { email });
        }

        public async Task<bool> IsAdmin(int id)
        {
            var sql = $"SELECT u.Role FROM Users u WHERE u.Id = {id}";
            return await DbConnection.QuerySingleOrDefaultAsync<string>(sql) == RoleConstants.Admin;
        }


        public async Task<Domain.DTO.UserDto> FindByEmployeeIdAsync(int employeeId)
        {
            var sql = $@"SELECT u.*, e.* 
                        FROM Users u LEFT JOIN Employees e ON u.EmployeeId = e.Id 
                        WHERE u.EmployeeId = {employeeId}";

            return await DbConnection.QuerySingleOrDefaultAsync<Domain.DTO.UserDto>(sql);
        }

        public async Task<Domain.DTO.UserDto> FindByIdWithEmployeeDataAsync(int id)
        {
            var sql = $@"SELECT u.Id, u.EmployeeId, u.Email, u.HasEmailConfirm, u.Role, e.FirstName, e.LastName, e.MiddleName, e.Salary, e.DepartmentID
                        FROM Users u LEFT JOIN Employees e ON u.EmployeeId = e.Id 
                        WHERE u.Id = {id}";

            return await DbConnection.QuerySingleOrDefaultAsync<Domain.DTO.UserDto>(sql);
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