using Anresh.Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Anresh.Domain.Repositories
{
    public interface IEmployeeSkillRepisitory : IGenericRepository<EmployeeSkill, int>
    {
        Task SaveMultipleAsync(List<Skill> skills, int employeeId);
        Task<IEnumerable<EmployeeSkill>> FindByEmployeeIdAsync(int employeeId);
    }
}
