using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Anresh.Application.Services.Department.Contracts;
using Anresh.Domain.DTO;
using Create = Anresh.Application.Services.Employee.Contracts.Create;
using Update = Anresh.Application.Services.Employee.Contracts.Update;

namespace Anresh.Application.Services.Employee.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetAllAsync();
        Task<Domain.Employee> GetByIdAsync(int id);
        Task<IEnumerable<EmployeeDto>> GetByDepartamentIdAsync(int id);
        Task<EmployeeDto> CreateAsync(Create request);
        Task<EmployeeDto> UpdateAsync(Update request);
        Task DeleteAsync(int id);
        Task DeleteMultipleAsync(IEnumerable<int> ids);
        Task DeleteAllByDepartmentIdAsync(int id);
    }
}
