using Anresh.Application.Services.Department.Contracts;
using Anresh.Domain;
using Anresh.Domain.DTO;
using Anresh.Domain.Repositories;
using Dapper;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Anresh.DataAccess.Repositories
{
    public sealed class DepartmentRepository : GenericRepository<Department, int>, IDepartmentRepository
    {
        public DepartmentRepository(IDbConnection db) : base(db)
        {
        }

        public async Task<bool> CheckNameAsync(string name)
        {
            var sql = $"SELECT COUNT(1) FROM {TableName} WHERE Name = @name";
            return await DbConnection.ExecuteScalarAsync<bool>(sql, new { name });
        }

        public async Task<IEnumerable<DepartmentDto>> FindAllWithEmployeeCountAsync()
        {
            var sql = @"SELECT d.*, count(e.Id) as EmployeeCount
                        FROM Departments d LEFT OUTER JOIN Employees e ON d.Id = e.DepartmentID
                        GROUP BY d.Id, d.Name";

            return await DbConnection.QueryAsync<DepartmentDto>(sql);
        }
    }
}