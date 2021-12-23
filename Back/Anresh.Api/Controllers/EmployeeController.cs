using Anresh.Api.Controllers.Requests.Employee;
using Anresh.Application.Services.Employee.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Anresh.Domain.DTO;

namespace Anresh.Controllers
{
    [Route("api/employee")]
    [ApiController]
    //[Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAllAsync()
        //{
        //    return Ok(await _employeeService.GetAllAsync());
        //}
        [HttpPost("filtred")]
        public async Task<IActionResult> GetFiltredWithDepartmentNameAndSkillsAsync([FromBody] EmployeesFilter employeesFilter)
        {
            return Ok(await _employeeService.GetFiltredWithDepartmentNameAndSkillsAsync(employeesFilter));
        }

        [HttpGet]
        public async Task<IActionResult> GetPagedAsync([FromQuery] PageParams pageParams)
        {
            return Ok(await _employeeService.GetPagedAsync(pageParams));
        }

        [HttpGet("totalRows")]
        public async Task<IActionResult> GetTotalRows()
        {
            return Ok(await _employeeService.GetTotalRows());
        }

        [HttpGet("all/department/{id}")]
        public async Task<IActionResult> GetByDepartamentAsync(int id)
        {
            return Ok(await _employeeService.GetAllByDepartamentIdAsync(id));
        }

        [HttpGet("department/{id}")]
        public async Task<IActionResult> GetByDepartamentIdPagedAsyncAsync([FromQuery] PageParams pageParams ,int id)
        {
            return Ok(await _employeeService.GetByDepartamentIdPagedAsyncAsync(pageParams, id));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateEmployeeRequest request)
        {
            var response = await _employeeService.CreateAsync(new EmployeeDto()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                MiddleName = request.MiddleName,
                DepartmentId = request.DepartmentId,
                Salary = request.Salary,
                Skills = request.Skills
            });
            return Created($"api/employee/{response.Id}", new { response });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateEmployeeRequest request)
        {
            var response = await _employeeService.UpdateAsync(new EmployeeDto()
            {
                Id = request.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                MiddleName = request.MiddleName,
                DepartmentId = request.DepartmentId,
                Salary = request.Salary,
                Skills = request.Skills
            });
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            await _employeeService.DeleteAsync(id);
            return NoContent();
        }

        [HttpDelete("multiple")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteMultipleAsync([FromBody] List<int> listId)
        {
            await _employeeService.DeleteMultipleAsync(listId);
            return NoContent();
        }

        [HttpDelete("department/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAllByDepartmentIdAsync([FromRoute] int id)
        {
            await _employeeService.DeleteAllByDepartmentIdAsync(id);
            return NoContent();
        }
    }
}
