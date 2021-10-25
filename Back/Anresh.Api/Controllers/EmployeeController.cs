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
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            return Ok(await _employeeService.GetAll(cancellationToken));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([Required] int id, CancellationToken cancellationToken)
        {
            return Ok(await _employeeService.GetById(id, cancellationToken));
        }
        [HttpGet("department/{id}")]

        public async Task<IActionResult> GetByDepartament([Required] int id, CancellationToken cancellationToken)
        {
            return Ok(await _employeeService.GetByDepartamentId(id, cancellationToken));
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeRequest request, CancellationToken cancellationToken)
        {
            var response = await _employeeService.Create(new Create.Request()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                MidleName = request.MidleName,
                DepartmentId = request.DepartmentId,
                Salary = request.Salary
            }, cancellationToken);
            return Created($"api/employee/{response.Id}", new { response });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateEmployeeRequest request, CancellationToken cancellationToken)
        {
           var response = await _employeeService.Update(new Update.Request()
           {
               Id = request.Id,
               FirstName = request.FirstName,
               LastName = request.LastName,
               MidleName = request.MidleName,
               DepartmentId = request.DepartmentId,
               Salary = request.Salary
           }, cancellationToken);
           return Ok(response);
        }
        [HttpPut("transferToTheDepartment")]
        public async Task<IActionResult> TransferToTheDepartment([FromBody][Required] TransferToTheDepartmentRequest request, CancellationToken cancellationToken)
        {
            await _employeeService.TransferToTheDepartment(request.OldDepartmentId, request.NewDepartmentId, cancellationToken);
            return Ok(request.NewDepartmentId);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete([FromBody] int id, CancellationToken cancellationToken)
        {
            await _employeeService.Delete(id, cancellationToken);
            return NoContent();
        }
        [HttpDelete("deleteList")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteList([FromBody] List<int> listId, CancellationToken cancellationToken)
        {
            await _employeeService.DeleteList(listId, cancellationToken);
            return NoContent();
        }

        [HttpDelete("deleteByDepartment")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAllByDepartmentId([FromBody][Required] int id, CancellationToken cancellationToken)
        {
            await _employeeService.DeleteAllByDepartmentId(id, cancellationToken);
            return NoContent();
        }
    }
}
