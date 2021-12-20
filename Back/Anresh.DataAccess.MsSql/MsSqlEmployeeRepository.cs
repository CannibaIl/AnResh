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
                         OFFSET {pageParams.Skip} ROWS FETCH NEXT {pageParams.take} ROWS ONLY;
                         
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


        public async Task<EmployeesFiltredPage> FindFiltredWithDepartmentNameAndSkillsAsync(EmployeesFilter employeesFilter)
        {
            var stringOfIds = employeesFilter.ListSkillsId is null || employeesFilter.ListSkillsId.Count == 0 ? null 
                : string.Join(", ", employeesFilter.ListSkillsId);

            var sql = $@"DECLARE @DepartmentID INT,@SkillId INT, @FirstName NVARCHAR(20), @MinSalary DECIMAL(18,2), @MaxSalary INT
                         SET @DepartmentID = {employeesFilter.DepartmentID ?? 0}
                         SET @FirstName = '{employeesFilter.FirstName}'
                         SET @MinSalary = {employeesFilter.MinSalary ?? 0}
                         SET @MaxSalary = {employeesFilter.MaxSalary ?? 0}

                         DECLARE @EmployeesFiltred TABLE
                         (Id INT, FirstName NVARCHAR(20), LastName NVARCHAR(20), MiddleName NVARCHAR(20),
                         DepartmentID INT, Salary DECIMAL, DepartmentName  NVARCHAR(20), SkillsCount INT);

                         DECLARE @EmployeesSorted TABLE
                         (Id INT, FirstName NVARCHAR(20), LastName NVARCHAR(20), MiddleName NVARCHAR(20),
                         DepartmentID INT, Salary DECIMAL, DepartmentName  NVARCHAR(20), SkillsCount INT);

                         WITH EmployeesFiltred AS
                         (
                         	SELECT e.Id
                         		 , e.FirstName
                         		 , e.LastName
                         		 , e.MiddleName
                         		 , e.DepartmentID
                         		 , e.Salary
                         		 , d.Name as DepartmentName
                         		 , (SELECT count(es.Id) FROM EmployeesSkills es WHERE e.Id = es.EmployeeId) as SkillsCount
                         	FROM Employees e
                         	LEFT JOIN Departments d ON e.DepartmentID = d.Id

                         	WHERE
                                (@DepartmentID = 0 or e.DepartmentID = @DepartmentID)
                         		AND e.FirstName like '%' + @FirstName + '%'
                         		AND e.Salary BETWEEN @MinSalary AND IIF(@MaxSalary = 0, (SELECT MAX(Salary) FROM Employees), @MaxSalary)
                                {(stringOfIds is null ? "" : $@"
                                    AND e.Id IN (SELECT es.EmployeeId FROM EmployeesSkills es WHERE es.SkillId IN ({stringOfIds})
				                    GROUP BY es.EmployeeId
                                    HAVING COUNT(es.EmployeeId) >= {employeesFilter.ListSkillsId.Count} )
                                ")}
                         )

                         INSERT INTO @EmployeesFiltred SELECT * FROM EmployeesFiltred;

                         INSERT INTO @EmployeesSorted SELECT * FROM @EmployeesFiltred
                         ORDER BY {employeesFilter.OrderBy ?? "FirstName"} {employeesFilter.AscDesc ?? "ASC"}
                         OFFSET {employeesFilter.Skip} ROWS FETCH NEXT {employeesFilter.take} ROWS ONLY;

                         SELECT * FROM @EmployeesSorted;

                         SELECT e.Id as EmployeeId, es.SkillId ,s.Name
                         FROM @EmployeesSorted e
                         JOIN EmployeesSkills es ON e.Id = es.EmployeeId
                         JOIN Skills s ON es.SkillId = s.Id

                         SELECT COUNT(Id) FROM @EmployeesFiltred;
                         ";

            var multi = await DbConnection.QueryMultipleAsync(sql).ConfigureAwait(false);

            var employees = multi.Read<EmployeeDto>().ToList();
            
            multi.Read<SkillDto>().GroupBy(x => x.EmployeeId,
                skill => new Skill() 
                { 
                    Id = skill.SkillId,
                    Name = skill.Name 
                })
                .ToList()
                .ForEach(skills => employees.First(e => e.Id == skills.Key).Skills = skills.ToList());

            var total = multi.Read<int>().First();

            return new EmployeesFiltredPage() 
            {
                Employees = employees,
                Total = total,
                TakeCount = employees.Count
            };
        }

    }
}