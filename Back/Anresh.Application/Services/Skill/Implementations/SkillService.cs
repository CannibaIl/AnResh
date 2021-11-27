using Anresh.Application.Services.Skill.Contracts;
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

        public async Task<Domain.Skill> CreateAsync(Create request)
        {
            if (await _skillRepository.CheckNameAsync(request.Name))
            {
                throw new Exception($"Skill {request.Name} already created!");
            }

            var skill = new Domain.Skill()
            {
                Name = request.Name
            };

            var id = await _skillRepository.SaveAsync(skill);
            skill.Id = id;

            return skill;
        }

        public async Task<Domain.Skill> UpdateAsync(Update request)
        {
            var skill = await _skillRepository.FindByIdAsync(request.Id);
            if (skill == null)
            {
                throw new KeyNotFoundException($"Skill with id:{request.Id} not found");
            }

            skill.Name = request.Name;

            await _skillRepository.UpdateAsync(skill);

            return skill;
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