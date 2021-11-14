using Anresh.Api.Controllers.Requests.Employee;
using Anresh.Application.Services.Employee.Contracts;
using Anresh.Application.Services.Employee.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Anresh.Controllers
{
    [Route("api/employee")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _employeeService.GetAll());
        }

        [HttpGet("department/{id}")]
        public async Task<IActionResult> GetByDepartament([Required] int id)
        {
            return Ok(await _employeeService.GetByDepartamentId(id));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeRequest request)
        {
            var response = await _employeeService.Create(new Create.Request()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                MiddleName = request.MiddleName,
                DepartmentId = request.DepartmentId,
                Salary = request.Salary
            });
            return Created($"api/employee/{response.Id}", new { response });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateEmployeeRequest request)
        {
           var response = await _employeeService.Update(new Update.Request()
           {
               Id = request.Id,
               FirstName = request.FirstName,
               LastName = request.LastName,
               MiddleName = request.MiddleName,
               DepartmentId = request.DepartmentId,
               Salary = request.Salary
           });
           return Ok(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _employeeService.Delete(id);
            return NoContent();
        }

        [HttpDelete("multiple")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteMultiple([FromBody] List<int> listId)
        {
            await _employeeService.DeleteMultiple(listId);
            return NoContent();
        }

        [HttpDelete("department/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAllByDepartmentId([FromRoute] int id)
        {
            await _employeeService.DeleteAllByDepartmentId(id);
            return NoContent();
        }
    }
}
