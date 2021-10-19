using Anresh.Domain;
using Anresh.Domain.DTO;
using Anresh.Domain.Repositories;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Anresh.DataAccess.Repositories
{
    public sealed class DepartmentRepository : GenericRepository<Department, int>, IDepartmentRepository
    {
        public DepartmentRepository(IDbConnection db) : base(db, TableNames.Departments)
        {
        }

        public async Task<bool> CheckName(string name, CancellationToken cancellationToken)
        {
            var department = await DbConnection.QueryFirstOrDefaultAsync<Department>($"SELECT * FROM {TableName} WHERE Name = '{name}'", cancellationToken);
            if (department == null)
                return false;
            else
                return true;
        }

        public async Task<IEnumerable<DepartmentDTO>> FindAllWithEmployees(CancellationToken cancellationToken)
        {
            string sql = $@"SELECT d.*, e.* FROM {TableNames.Departments} d LEFT JOIN {TableNames.Employees} e ON d.Id = e.DepartmentId";

            var dictionary = new Dictionary<int, DepartmentDTO>();

            var departments = await DbConnection.QueryAsync<DepartmentDTO, Employee, DepartmentDTO>(
                sql, (d, e) => {
                    if (!dictionary.TryGetValue(d.Id, out DepartmentDTO department))
                        dictionary.Add(d.Id, department = d);

                    if (department.Employees == null)
                        department.Employees = new List<Employee>();
                    
                    department.Employees.Add(e);
                    return department;
                }, cancellationToken);
    
            return dictionary.Values;
        }

        public async Task<DepartmentDTO> FindByIdWithEmployees(int id, CancellationToken cancellationToken)
        {
            string sql = $@"SELECT d.*, e.* FROM {TableNames.Departments} d LEFT JOIN {TableNames.Employees} e ON d.Id = e.DepartmentId WHERE d.Id = {id} ";

            var dictionary = new Dictionary<int, DepartmentDTO>();

            var departments = await DbConnection.QueryAsync<DepartmentDTO, Employee, DepartmentDTO>(
                sql, (d, e) => {
                    if (!dictionary.TryGetValue(d.Id, out DepartmentDTO department))
                        dictionary.Add(d.Id, department = d);

                    if (department.Employees == null)
                        department.Employees = new List<Employee>();

                    department.Employees.Add(e);
                    return department;
                }, cancellationToken);

            return dictionary.FirstOrDefault().Value;
        }
    }
}
