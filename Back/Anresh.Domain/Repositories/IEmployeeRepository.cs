using System.Collections.Generic;
using System.Threading.Tasks;

namespace Anresh.Domain.Repositories
{
    public interface IEmployeeRepository : IGenericRepository<Employee, int> {
        Task<IEnumerable<Employee>> FindAllByDepartmentId(int id);
        Task TransferToDepartment(int oldDepartmentId, int newDepartmentId);
        Task DeleteByDepartmentId(int id);
        IDictionary<int, int> CountAndGroupByDepartment();
        int CountByDepartmentId(int id);
    }
}
