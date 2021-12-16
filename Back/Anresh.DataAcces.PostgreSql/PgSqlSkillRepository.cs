using Anresh.DataAccess.MsSql.Repositories;
using Anresh.Domain;
using Anresh.Domain.Repositories;
using Dapper;
using System.Data;
using System.Threading.Tasks;

namespace Anresh.DataAccess.MsSql.Repositories
{
    public class PgSqlSkillRepository : PgSqlGenericRepository<Skill, int>, ISkillRepository
    {
        public PgSqlSkillRepository(IDbConnection db) : base(db)
        {
        }
        public async Task<bool> CheckNameAsync(string name)
        {
            var sql = $"SELECT COUNT(1) FROM {TableName} WHERE Name = @name";
            return await DbConnection.ExecuteScalarAsync<bool>(sql, new { name });
        }
    }
}