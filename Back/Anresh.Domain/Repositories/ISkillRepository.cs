using System.Threading.Tasks;

namespace Anresh.Domain.Repositories
{
    public interface ISkillRepository : IGenericRepository<Skill, int>
    {
        Task<bool> CheckNameAsync(string name);
    }
}
