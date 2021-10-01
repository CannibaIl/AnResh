using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Anresh.Application.Services.Department.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Domain.Department>> GetAll(CancellationToken cancellationToken);
    }
}
