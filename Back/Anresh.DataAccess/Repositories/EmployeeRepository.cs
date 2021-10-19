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
    public sealed class EmployeeRepository : GenericRepository<Employee, int>, IEmployeeRepository
    {
        public EmployeeRepository(IDbConnection db) : base(db, TableNames.Employees)
        {
        }
        public async Task<List<Employee>> FindByDepartmentId(int id, CancellationToken cancellationToken)
        {
            var data = await DbConnection.QueryAsync<Employee>($"SELECT * FROM {TableName} WHERE DepartmentID = {id}", cancellationToken);
            return data.ToList();
        }

        public async Task TransferToTheDepartment(int oldDepartmentId, int newDepartmentId, CancellationToken cancellationToken)
        {
            await DbConnection.QueryAsync<Employee>($"UPDATE {TableName} SET DepartmentId = {newDepartmentId} WHERE DepartmentId = {oldDepartmentId}", cancellationToken);
        }
        public async Task DeleteByDepartmentId(int id, CancellationToken cancellation)
        {
            await DbConnection.ExecuteAsync($"delete from {TableName} where DepartmentId = {id}", cancellation);
        }

        public async Task<IEnumerable<EmployeeDTO>> FindAllWithDepartment(CancellationToken cancellationToken)
        {
            var sql = $@"SELECT * FROM {TableNames.Employees} e 
                        LEFT JOIN {TableNames.Departments} d 
                        ON d.Id = e.DepartmentId 
                        ORDER BY e.Id";

            var employees = await DbConnection.QueryAsync<EmployeeDTO, Department, EmployeeDTO>(sql, (employee, department) => { 
                employee.Department = department; return employee; 
            }, cancellationToken);

            return employees.ToList();
        }

        public async Task<IEnumerable<EmployeeDTO>> FindByDepartmentIdWithDepartment(int id, CancellationToken cancellationToken)
        {
            var sql = $@"SELECT * FROM {TableNames.Employees} e 
                        LEFT JOIN {TableNames.Departments} d 
                        ON e.DepartmentId = d.Id 
                        WHERE e.DepartmentId = {id} 
                        ORDER BY e.Id";

            var employees = await DbConnection.QueryAsync<EmployeeDTO, Department, EmployeeDTO>(sql, (employee, department) => {
                employee.Department = department; return employee;
            }, cancellationToken);

            return employees;
        }

        public async Task<EmployeeDTO> FindByIdWithDepartment(int id, CancellationToken cancellationToken)
        {
            var sql = $@"SELECT * FROM {TableNames.Employees} e 
                        LEFT JOIN {TableNames.Departments} d 
                        ON e.DepartmentId = d.Id 
                        WHERE e.Id = {id} 
                        ORDER BY e.Id";

            var employee = await DbConnection.QueryAsync<EmployeeDTO, Department, EmployeeDTO>(sql, (employee, department) => { 
                employee.Department = department; return employee; 
            }, cancellationToken);

            return employee.FirstOrDefault();
        }
    }
}
