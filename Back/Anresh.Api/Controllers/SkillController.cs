using Anresh.Api.Controllers.Requests.Skill;
using Anresh.Application.Services.Skill.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Anresh.Controllers
{
    [Route("api/skill")]
    [ApiController]
    //[Authorize]
    public class SkillController : Controller
    {
        private readonly ISkillService _skillService;
        public SkillController(ISkillService skillService)
        {
            _skillService = skillService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _skillService.GetAllAsync());
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateSkillRequest request)
        {
            var response = await _skillService.CreateAsync(new Domain.Skill()
            {
                Name = request.Name
            });
            return Created($"api/skill/{response.Id}", new { response });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateSkillRequest request)
        {
            var response = await _skillService.UpdateAsync(new Domain.Skill()
            {
                Id = request.Id,
                Name = request.Name
            });

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            await _skillService.DeleteAsync(id);
            return NoContent();
        }
    }
}
