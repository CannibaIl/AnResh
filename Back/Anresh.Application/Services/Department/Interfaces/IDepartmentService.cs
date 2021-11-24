using Anresh.Application.Services.Department.Contracts;
using Anresh.Domain.DTO;
using Anresh.Domain.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Anresh.Application.Services.Department.Interfaces
{
    public interface IDepartmentService
    {
        Task<Domain.Department> GetSimpleByIdAsync(int id);
        Task<IEnumerable<DepartmentDto>> GetAllAsync();
        Task<IEnumerable<Domain.Department>> GetAllSimpleAsync();
        Task<Domain.Department> CreateAsync(Create request);
        Task<Domain.Department> UpdateAsync(Update request);
        Task DeleteAsync(int id);
        Task MoveEmployeesAsync(int oldDepartmentId, int newDepartmentId);
    }
}
