using System.Threading.Tasks;

namespace Anresh.Domain.Repositories
{
    public interface IDepartmentRepository : IGenericRepository<Department, int>
    {
        Task<bool> CheckName(string name);
    }
}
