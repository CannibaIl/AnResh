using Anresh.Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Anresh.Domain.Repositories
{
    public interface IDepartmentRepository : IGenericRepository<Department, int>
    {
        Task<bool> CheckNameAsync(string name);
        Task<IEnumerable<DepartmentDto>> FindAllWithEmployeeCountAsync();
    }
}
