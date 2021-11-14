using System.ComponentModel.DataAnnotations;
using Anresh.Api.Controllers.Requests.Department;
using Anresh.Application.Services.Department.Contracts;
using Anresh.Application.Services.Department.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Anresh.Api.Controllers.Requests.Employee;

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
        
        [HttpGet("simple/{id}")]
        public async Task<IActionResult> GetSimpleById([FromRoute] int id)
        {
            return Ok(await _departmentService.GetSimpleById(id));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _departmentService.GetAll());
        }

        [HttpGet("options")]
        public async Task<IActionResult> GetAllSimple()
        {
            return Ok(await _departmentService.GetAllAsOptions());
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateDepartmentRequest request)
        {
            var response = await _departmentService.Create(new Create.Request()
            {
                Name = request.Name
            });
            return Created($"api/department/{response.Id}", new { response });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateDepartmentRequest request)
        {
            var response = await _departmentService.Update(new Update.Request()
            {
                Id = request.Id,
                Name = request.Name
            });

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        { 
            await _departmentService.Delete(id);
            return NoContent();
        }

        [HttpPut("move-employees")]
        public async Task<IActionResult> MoveEmployees([FromBody][Required] TransferToTheDepartmentRequest request)
        {
            var result = await _departmentService.MoveEmployees(request.OldDepartmentId, request.NewDepartmentId);
            return Ok(result);
        }

    }
}
