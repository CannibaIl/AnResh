using Anresh.Application.Services.Department.Contracts;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Anresh.Application.Services.Department.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Domain.DTO.DepartmentDTO>> GetAll(CancellationToken cancellationToken);
        Task<IEnumerable<Domain.Department>> GetAllLight(CancellationToken cancellationToken);
        Task<Domain.DTO.DepartmentDTO> Create(Create.Request request, CancellationToken cancellationToken);
        Task<Domain.DTO.DepartmentDTO> Update(Update.Request request, CancellationToken cancellationToken);
        Task Delete(int id, CancellationToken cancellationToken);
    }
}
