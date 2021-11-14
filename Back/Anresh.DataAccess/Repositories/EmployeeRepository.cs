using System;
using Anresh.Domain;
using Anresh.Domain.Repositories;
using Dapper;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Anresh.DataAccess.Repositories
{
    public sealed class EmployeeRepository : GenericRepository<Employee, int>, IEmployeeRepository
    {
        public EmployeeRepository(IDbConnection db)
            : base(db, TableNames.Employees)
        {
        }

        public async Task<IEnumerable<Employee>> FindAllByDepartmentId(int id)
        {
            var sql = $"SELECT * FROM {TableName} WHERE DepartmentID = @id";
            return await DbConnection.QueryAsync<Employee>(sql, new { id });
        }

        public async Task TransferToDepartment(int oldDepartmentId, int newDepartmentId)
        {
            var sql = $"UPDATE {TableName} SET DepartmentId = @newDepartmentId WHERE DepartmentId = @oldDepartmentId";
            await DbConnection.QueryAsync<Employee>(sql, new { newDepartmentId, oldDepartmentId });
        }

        public async Task DeleteByDepartmentId(int id)
        {
            var sql = $"delete from {TableName} where DepartmentId = @id";
            await DbConnection.ExecuteAsync(sql, new { id });
        }

        public IDictionary<int, int> CountAndGroupByDepartment()
        {
            var sql = $"select DepartmentId, count(DepartmentId) as Count from {TableName} group by DepartmentId";
            return DbConnection.Query(sql).ToDictionary(x => (int)x.DepartmentId, x => (int)x.Count);
        }

        public int CountByDepartmentId(int id)
        {
            var sql = $"select count(Id) from {TableName} where DepartmentId = @id";
            return DbConnection.ExecuteScalar<int>(sql, new { id });
        }
    }
}