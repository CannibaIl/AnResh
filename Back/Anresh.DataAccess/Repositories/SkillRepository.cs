using Anresh.Domain;
using Anresh.Domain.Repositories;
using Dapper;
using System.Data;
using System.Threading.Tasks;

namespace Anresh.DataAccess.Repositories
{
    public sealed class SkillRepository : GenericRepository<Skill, int>, ISkillRepository
    {
        public SkillRepository(IDbConnection db) : base(db)
        {
        }
        public async Task<bool> CheckNameAsync(string name)
        {
            var sql = $"SELECT COUNT(1) FROM {TableName} WHERE Name = @name";
            return await DbConnection.ExecuteScalarAsync<bool>(sql, new { name });
        }
    }
}