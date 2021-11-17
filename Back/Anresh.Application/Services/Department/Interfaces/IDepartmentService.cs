using Anresh.Application.Services.Department.Contracts;
using Anresh.Domain.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Anresh.Application.Services.Department.Interfaces
{
    public interface IDepartmentService
    {
        //если используешь таски, используй и стандарт именования методов: GetSimpleByIdAsync
        Task<Domain.Department> GetSimpleById(int id);
        Task<IEnumerable<DepartmentDto>> GetAll();
        Task<IEnumerable<OptionDto>> GetAllAsOptions();
        Task<Domain.Department> Create(Create.Request request);
        Task<Domain.Department> Update(Update.Request request);
        Task Delete(int id);
        Task<DepartmentDto> MoveEmployees(int oldDepartmentId, int newDepartmentId);
    }
}
