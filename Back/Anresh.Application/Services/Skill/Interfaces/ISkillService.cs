using Anresh.Application.Services.Skill.Contracts;
using Anresh.Domain.DTO;
using Anresh.Domain.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Anresh.Application.Services.Skill.Interfaces
{
    public interface ISkillService
    {
        Task<IEnumerable<Domain.Skill>> GetAllAsync();
        Task<Domain.Skill> CreateAsync(Create request);
        Task<Domain.Skill> UpdateAsync(Update request);
        Task DeleteAsync(int id);
    }
}
