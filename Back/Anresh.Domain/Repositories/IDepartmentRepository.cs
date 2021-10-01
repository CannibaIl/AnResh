using Anresh.Domain;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Anresh.Domain.Repositories
{
    public interface IDepartmentRepository : IGenericRepository<Department, int>
    {
    }
}
