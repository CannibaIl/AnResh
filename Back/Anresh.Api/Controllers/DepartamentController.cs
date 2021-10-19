using Anresh.Api.Controllers.Requests.Department;
using Anresh.Application.Services.Department.Contracts;
using Anresh.Application.Services.Department.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Anresh.Controllers
{
    [Route("api/department")]
    [ApiController]
    public class DepartamentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        public DepartamentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            return Ok(await _departmentService.GetAll(cancellationToken));
        }
        [HttpGet("light")]
        public async Task<IActionResult> GetAllLight(CancellationToken cancellationToken)
        {
            return Ok(await _departmentService.GetAllLight(cancellationToken));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateDepartmentRequest request, CancellationToken cancellationToken)
        {
            var response = await _departmentService.Create(new Create.Request()
            {
                Name = request.Name
            }, cancellationToken);
            return Created($"api/department/{response.Id}", new { response });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateDepartmentRequest request, CancellationToken cancellationToken)
        {
            var response = await _departmentService.Update(new Update.Request()
            {
                Id = request.Id,
                Name = request.Name
            }, cancellationToken);

            return Ok(response);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete([FromBody] [Required]int id, CancellationToken cancellationToken)
        {
            await _departmentService.Delete(id, cancellationToken);
            return NoContent();
        }
    }
}
