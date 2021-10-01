using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Anresh.Application.Services.Employee.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Domain.Employee>> GetAll(CancellationToken cancellationToken);
        Task<IEnumerable<Domain.Employee>> GetByDepartamentId(int id, CancellationToken cancellationToken);
    }
}
