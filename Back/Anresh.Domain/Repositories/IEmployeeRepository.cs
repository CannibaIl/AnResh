using Anresh.Domain;
using Anresh.Domain.DTO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Anresh.Domain.Repositories
{
    public interface IEmployeeRepository : IGenericRepository<Employee, int>
    {
        Task<IEnumerable<EmployeeDTO>> FindAllWithDepartment(CancellationToken cancellationToken);
        Task<IEnumerable<EmployeeDTO>> FindByDepartmentIdWithDepartment(int id, CancellationToken cancellationToken);
        Task<EmployeeDTO> FindByIdWithDepartment(int id, CancellationToken cancellationToken);
        Task<List<Employee>> FindByDepartmentId(int id, CancellationToken cancellationToken);
        Task TransferToTheDepartment(int oldDepartmentId, int newDepartmentId, CancellationToken cancellationToken);
        Task DeleteByDepartmentId(int id, CancellationToken cancellation);
    }
}
