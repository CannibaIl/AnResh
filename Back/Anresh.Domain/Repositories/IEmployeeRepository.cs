using Anresh.Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Anresh.Domain.Repositories
{
    public interface IEmployeeRepository : IGenericRepository<Employee, int> {
        Task<IEnumerable<EmployeeDto>> FindAllByDepartmentIdWithDepartmentNameAsync(int id);
        Task TransferToDepartmentAsync(int oldDepartmentId, int newDepartmentId);
        Task DeleteByDepartmentIdAsync(int id);
        Task<IEnumerable<EmployeeDto>> FindWithDepartmentNameAndSkillsAsync(PageParams pageParams, int? departmentId = null);
    }
}
