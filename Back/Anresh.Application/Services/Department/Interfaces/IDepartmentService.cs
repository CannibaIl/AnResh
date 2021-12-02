using Anresh.Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Anresh.Application.Services.Department.Interfaces
{
    public interface IDepartmentService
    {
        Task<Domain.Department> GetSimpleByIdAsync(int id);
        Task<IEnumerable<DepartmentDto>> GetPagedAsync(PageParams pageParams);
        Task<IEnumerable<Domain.Department>> GetAllSimpleAsync();
        Task<Domain.Department> CreateAsync(Domain.Department request);
        Task<Domain.Department> UpdateAsync(Domain.Department request);
        Task DeleteAsync(int id);
        Task MoveEmployeesAsync(int oldDepartmentId, int newDepartmentId);
        Task<int> GetTotalRows();
    }
}
