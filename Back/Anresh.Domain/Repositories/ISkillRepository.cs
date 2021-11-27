using Anresh.Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Anresh.Domain.Repositories
{
    public interface ISkillRepository : IGenericRepository<Skill, int>
    {
        Task<bool> CheckNameAsync(string name);
    }
}
