using Anresh.Domain;
using Anresh.Domain.Repositories;
using Anresh.Domain.Shared;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Anresh.DataAccess.Repositories
{
    public sealed class EmployeeSkillRepisitory : GenericRepository<EmployeeSkill, int>, IEmployeeSkillRepisitory
    {
        public EmployeeSkillRepisitory(IDbConnection db) : base(db)
        {
        }
        public async Task<IEnumerable<EmployeeSkill>> FindByEmployeeIdAsync(int employeeId)
        {
            var sql = $"SELECT * FROM {TableName} WHERE EmployeeId = @employeeId";
            return await DbConnection.QueryAsync<EmployeeSkill>(sql, new { employeeId });
        }
        public async Task SaveMultipleAsync(List<Skill> skills, int employeeId)
        {
            var columnNames = string.Join(", ", new EmployeeSkill().GetColumns());
            var employeesSkilsValues = string.Join(", ", skills.Select(skill => $"({employeeId}, {skill.Id})"));
            var sql = $"insert into { TableName } ({columnNames}) values {employeesSkilsValues}";
            
            await DbConnection.QueryAsync<int>(sql);
        }
    }
}