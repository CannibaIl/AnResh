using Anresh.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Anresh.Domain.Repositories
{
    public interface IEmployeeRepository : IGenericRepository<Employee, int>
    {
        Task<List<Employee>> FindByDepartmentId(int id, CancellationToken cancellationToken);
    }
}
