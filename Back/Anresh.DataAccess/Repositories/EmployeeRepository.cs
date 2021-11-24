using System;
using Anresh.Domain;
using Anresh.Domain.Repositories;
using Dapper;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Anresh.Domain.DTO;

namespace Anresh.DataAccess.Repositories
{
    public sealed class EmployeeRepository : GenericRepository<Employee, int>, IEmployeeRepository
    {
        public EmployeeRepository(IDbConnection db) : base(db)
        {
        }

        public async Task<IEnumerable<EmployeeDto>> FindAllByDepartmentIdWithDepartmentNameAsync(int id)
        {
            var sql = $@"SELECT e.*, d.Name as DepartmentName
                         FROM Employees e LEFT OUTER JOIN Departments d ON e.DepartmentID = d.Id
                         WHERE e.DepartmentID = @id
                         GROUP BY e.Id, e.LastName, e.FirstName, e.MiddleName, e.Salary, e.DepartmentID, d.Name";

            return await DbConnection.QueryAsync<EmployeeDto>(sql, new { id });
        }

        public async Task TransferToDepartmentAsync(int oldDepartmentId, int newDepartmentId)
        {
            var sql = $"UPDATE {TableName} SET DepartmentId = @newDepartmentId WHERE DepartmentId = @oldDepartmentId";
            await DbConnection.QueryAsync<Employee>(sql, new { newDepartmentId, oldDepartmentId });
        }

        public async Task DeleteByDepartmentIdAsync(int id)
        {
            var sql = $"delete from {TableName} where DepartmentId = @id";
            await DbConnection.ExecuteAsync(sql, new { id });
        }

        public async Task<IEnumerable<EmployeeDto>> FindAllWithDepartmentNameAsync()
        {
            var sql = @"SELECT e.*, d.Name as DepartmentName
                        FROM Employees e LEFT OUTER JOIN Departments d ON e.DepartmentID = d.Id
                        GROUP BY e.LastName, e.FirstName, e.MiddleName, e.Salary, e.Id, e.DepartmentID, d.Name";

            return await DbConnection.QueryAsync<EmployeeDto>(sql);
        }

    }
}