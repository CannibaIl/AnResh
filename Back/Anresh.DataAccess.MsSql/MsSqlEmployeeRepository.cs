using Anresh.Domain;
using Anresh.Domain.DTO;
using Anresh.Domain.Repositories;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Anresh.DataAccess.MsSql.Repositories
{
    public class MsSqlEmployeeRepository : MsSqlGenericRepository<Employee, int>, IEmployeeRepository
    {
        public MsSqlEmployeeRepository(IDbConnection db) : base(db)
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

        public async Task<IEnumerable<EmployeeDto>> FindWithDepartmentNameAndSkillsAsync(PageParams pageParams, int? departmentId)
        {
            var sql = $@"CREATE TABLE #EmployeesDetails
                         (Id INT, FirstName NVARCHAR(20), LastName NVARCHAR(20), MiddleName NVARCHAR(20),
                         DepartmentID INT, Salary DECIMAL, DepartmentName  NVARCHAR(20), SkillsCount INT)

                         INSERT INTO #EmployeesDetails
                         SELECT e.*, d.Name as DepartmentName, count(es.Id) as SkillsCount
                         FROM Employees e JOIN Departments d ON e.DepartmentID = d.Id
                         LEFT JOIN EmployeesSkills es ON e.Id = es.EmployeeId

                         {(departmentId is not null ? $"WHERE e.DepartmentID = {departmentId}" : "")}

                         GROUP BY e.Id, e.FirstName, e.LastName, e.MiddleName, e.Salary, e.DepartmentID, d.Name
                         ORDER BY {pageParams.OrderBy} {pageParams.AscDesc}
                         OFFSET {pageParams.Skip} ROWS FETCH NEXT {pageParams.Take} ROWS ONLY;
                         
                         SELECT * FROM #EmployeesDetails;
                         
                         SELECT e.Id as EmployeeId, s.Id as SkillId ,s.Name
                         FROM #EmployeesDetails e 
                         JOIN EmployeesSkills es ON e.Id = es.EmployeeId
                         JOIN Skills s ON es.SkillId = s.Id; ";

            var multi = await DbConnection.QueryMultipleAsync(sql).ConfigureAwait(false);

            var employees = multi.Read<EmployeeDto>().ToList();
            var groupSkills = multi.Read<SkillDto>().GroupBy(x => x.EmployeeId,
                                                             skill => new Skill()
                                                             {
                                                                 Id = skill.SkillId,
                                                                 Name = skill.Name
                                                             }).ToList();
            groupSkills.ForEach(skills =>
                                employees.First(e => e.Id == skills.Key)
                               .Skills = skills.ToList());

            return employees;
        }

    }
}