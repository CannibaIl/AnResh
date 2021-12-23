using Anresh.Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Anresh.Application.Services.Employee.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetAllByDepartamentIdAsync(int id);
        Task<IEnumerable<EmployeeDto>> GetPagedAsync(PageParams pageConstructor);
        Task<IEnumerable<EmployeeDto>> GetByDepartamentIdPagedAsyncAsync(PageParams pageParams, int id);
        Task<EmployeesFiltredPage> GetFiltredWithDepartmentNameAndSkillsAsync(EmployeesFilter employeesFilter);
        Task<Domain.Employee> GetByIdAsync(int id);
        Task<EmployeeDto> CreateAsync(EmployeeDto request);
        Task<EmployeeDto> UpdateAsync(EmployeeDto request);
        Task DeleteAsync(int id);
        Task DeleteMultipleAsync(IEnumerable<int> ids);
        Task DeleteAllByDepartmentIdAsync(int id);
        Task<int> GetTotalRows();
    }
}