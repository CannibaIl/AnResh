using Anresh.Api.Controllers.Requests.Department;
using Anresh.Api.Controllers.Requests.Employee;
using Anresh.Application.Services.Department.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Anresh.Controllers
{
    [Route("api/department")]
    [ApiController]
    [Authorize]
    public class DepartamentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        public DepartamentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet("simple/{id}")]
        public async Task<IActionResult> GetSimpleByIdAsync([FromRoute] int id)
        {
            return Ok(await _departmentService.GetSimpleByIdAsync(id));
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAllAsync()
        //{
        //    return Ok(await _departmentService.GetAllAsync());
        //}
        [HttpGet]
        public async Task<IActionResult> GetPagedAsync([FromQuery] Domain.DTO.PageParams pageParams)
        {
            return Ok(await _departmentService.GetPagedAsync(pageParams));
        }

        [HttpGet("totalRows")]
        public async Task<IActionResult> GetTotalRows()
        {
            return Ok(await _departmentService.GetTotalRows());
        }

        [HttpGet("simple")]
        public async Task<IActionResult> GetAllSimple()
        {
            return Ok(await _departmentService.GetAllSimpleAsync());
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateDepartmentRequest request)
        {
            var response = await _departmentService.CreateAsync(new Domain.Department()
            {
                Name = request.Name
            });
            return Created($"api/department/{response.Id}", new { response });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateDepartmentRequest request)
        {
            var response = await _departmentService.UpdateAsync(new Domain.Department()
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
            await _departmentService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut("move-employees")]
        public async Task<IActionResult> MoveEmployeesAsync([FromBody] TransferToTheDepartmentRequest request)
        {
            await _departmentService.MoveEmployeesAsync(request.OldDepartmentId, request.NewDepartmentId);
            return Ok();
        }

    }
}
