using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Anresh.Application.Services.Department.Contracts;
using Create = Anresh.Application.Services.Employee.Contracts.Create;
using Update = Anresh.Application.Services.Employee.Contracts.Update;

namespace Anresh.Application.Services.Employee.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetAll();
        Task<Domain.Employee> GetById(int id);
        Task<IEnumerable<EmployeeDto>> GetByDepartamentId(int id);
        Task<Domain.Employee> Create(Create.Request request);
        Task<Domain.Employee> Update(Update.Request request);
        Task Delete(int id);
        Task DeleteMultiple(IEnumerable<int> ids);
        Task DeleteAllByDepartmentId(int id);
    }
}
