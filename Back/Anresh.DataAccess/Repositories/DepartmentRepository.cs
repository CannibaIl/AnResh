using Anresh.Domain;
using Anresh.Domain.Repositories;
using Dapper;
using System.Data;
using System.Threading.Tasks;

namespace Anresh.DataAccess.Repositories
{
    public sealed class DepartmentRepository : GenericRepository<Department, int>, IDepartmentRepository
    {
        public DepartmentRepository(IDbConnection db)
            : base(db, TableNames.Departments)
        {
        }

        public async Task<bool> CheckName(string name)
        {
            var sql = $"SELECT COUNT(1) FROM {TableName} WHERE Name = @name";
            return await DbConnection.ExecuteScalarAsync<bool>(sql, new { name });
        }
    }
}