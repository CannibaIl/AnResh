using System.Collections.Generic;
using System.Threading.Tasks;

namespace Anresh.Application.Services.Skill.Interfaces
{
    public interface ISkillService
    {
        Task<IEnumerable<Domain.Skill>> GetAllAsync();
        Task<Domain.Skill> CreateAsync(Domain.Skill request);
        Task<Domain.Skill> UpdateAsync(Domain.Skill request);
        Task DeleteAsync(int id);
    }
}
