using Anresh.Domain;
using Anresh.Domain.DTO;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Anresh.Domain.Repositories
{
    public interface IDepartmentRepository : IGenericRepository<Department, int>
    {
        Task<IEnumerable<DepartmentDTO>> FindAllWithEmployees(CancellationToken cancellationToken);
        Task<bool> CheckName(string name, CancellationToken cancellationToken);
        Task<DepartmentDTO> FindByIdWithEmployees(int id, CancellationToken cancellationToken);
    }
}
