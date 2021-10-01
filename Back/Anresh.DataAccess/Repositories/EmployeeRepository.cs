using Anresh.Domain;
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
            var data = await DbConnection.QueryAsync<Employee>($"SELECT * FROM Employees WHERE DepartmentID = {id}", cancellationToken);
            return data.ToList();
        }
    }
}
