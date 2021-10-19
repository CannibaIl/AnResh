using Anresh.Application.Services.Employee.Contracts;
using Anresh.Domain.DTO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Anresh.Application.Services.Employee.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDTO>> GetAll(CancellationToken cancellationToken);
        Task<Domain.Employee> GetById(int id, CancellationToken cancellationToken);
        Task<IEnumerable<EmployeeDTO>> GetByDepartamentId(int id, CancellationToken cancellationToken);
        Task<EmployeeDTO> Create(Create.Request request, CancellationToken cancellationToken);
        Task<EmployeeDTO> Update(Update.Request request, CancellationToken cancellationToken);
        Task Delete(int id, CancellationToken cancellationToken);
        Task DeleteList(List<int> listId, CancellationToken cancellationToken);
        Task DeleteAllByDepartmentId(int id, CancellationToken cancellationToken);
        Task TransferToTheDepartment(int oldDepartmentId, int newDepartmentId, CancellationToken cancellationToken);
    }
}
