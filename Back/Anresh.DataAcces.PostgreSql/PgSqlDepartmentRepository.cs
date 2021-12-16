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
    public class PgSqlDepartmentRepository : PgSqlGenericRepository<Department, int>, IDepartmentRepository
    {
        public PgSqlDepartmentRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }

        public async Task<bool> CheckNameAsync(string name)
        {
            var sql = $"SELECT COUNT(1) FROM {TableName} WHERE Name = @name";
            return await DbConnection.ExecuteScalarAsync<bool>(sql, new { name });
        }

        public async Task<IEnumerable<DepartmentDto>> FindWithEmployeeCountAsync(PageParams pageParams)
        {
            var sql = $@"SELECT d.*, count(e.Id) as EmployeeCount, AVG(e.Salary) AS AverageSalary
                         FROM Departments d LEFT OUTER JOIN Employees e ON d.Id = e.DepartmentID
                         GROUP BY d.Id, d.ParentId, d.Name
                         ORDER BY {pageParams.OrderBy} {pageParams.AscDesc}
                         OFFSET {pageParams.Skip} ROWS FETCH NEXT {pageParams.Take} ROWS ONLY;";

            return await DbConnection.QueryAsync<DepartmentDto>(sql);
        }

        public async Task<IEnumerable<DepartmentSimpleDto>> FindSimpleByParentIdAsync(int parentId)
        {
            var sql = $@"SELECT d.*
                         FROM Departments d
                         WHERE d.ParentId {(parentId == 0 ? "IS NULL" : $" = {parentId}")}
                         GROUP BY d.Id, d.ParentId, d.Name";

            return await DbConnection.QueryAsync<DepartmentSimpleDto>(sql);
        }

        public async Task<DepartmentSimpleChildrenAndParentsDto> FindSimpleParentsTreeAndParentChildrenByChildIdAsync(int childId)
        {
            var sql = $@"CREATE TABLE #parents ( Id INT , Name NVARCHAR(20), ParentId INT)
                         DECLARE @ParentId INT;
                         SET @ParentId = (SELECT ParentId FROM Departments WHERE Id = {childId})

                         SELECT * FROM Departments WHERE ISNULL(ParentId, 0) = ISNULL(@ParentId, 0);

                         WHILE @ParentId is not null
                         BEGIN
	                     	INSERT INTO #parents
                             SELECT * FROM Departments WHERE ISNULL(Id, 0) = ISNULL(@ParentId, 0)
	                     	SET @ParentId = (SELECT ParentId FROM Departments WHERE Id = @ParentId)
                         END;

                         SELECT * FROM #parents";

            var multi = await DbConnection.QueryMultipleAsync(sql);

            var children = multi.Read<DepartmentSimpleDto>();
            var parents = multi.Read<DepartmentSimpleDto>().Reverse();
            return new DepartmentSimpleChildrenAndParentsDto(children, parents);
        }

        public async Task<IEnumerable<DepartmentDto>> FindAllWithEmployeeCountAsync()
        {
            var sql = $@"SELECT d.*, count(e.Id) as EmployeeCount, AVG(e.Salary) AS AverageSalary
                         FROM Departments d LEFT JOIN Employees e ON d.Id = e.DepartmentID
                         GROUP BY d.Id, d.ParentId, d.Name";

            var departments = await DbConnection.QueryAsync<DepartmentDto>(sql);

            return MappingChildren(departments);
        }

        private static IEnumerable<DepartmentDto> MappingChildren(IEnumerable<DepartmentDto> departments)
        {
            var groups = departments.GroupBy(x => x.ParentId).ToList();
            foreach (var group in groups)
            {
                if (group.Key == 0)
                {
                    continue;
                }
                departments.First(d => d.Id == group.Key).Children = group.ToList();
            }
            return departments.Where(x => x.ParentId == 0);
        }
    }
}