using Anresh.Application.Services.Skill.Interfaces;
using Anresh.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Anresh.Application.Services.Skill.Implementations
{
    public sealed class SkillService : ISkillService
    {
        private readonly ISkillRepository _skillRepository;

        public SkillService(ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }

        public async Task<Domain.Skill> CreateAsync(Domain.Skill request)
        {
            if (await _skillRepository.CheckNameAsync(request.Name))
            {
                throw new Exception($"Skill {request.Name} already created!");
            }
            request.Id = await _skillRepository.SaveAsync(request);

            return request;
        }

        public async Task<Domain.Skill> UpdateAsync(Domain.Skill request)
        {
            if (await _skillRepository.IsExistsAsync(request.Id) == false)
            {
                throw new KeyNotFoundException($"Skill with id:{request.Id} not found");
            }
            await _skillRepository.UpdateAsync(request);

            return request;
        }

        public async Task DeleteAsync(int id)
        {
            await _skillRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Domain.Skill>> GetAllAsync()
        {
            return await _skillRepository.FindAllAsync();
        }
    }
}