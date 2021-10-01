using Anresh.Application.Services.Employee.Interfaces;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("department/{id}")]

        public async Task<IActionResult> GetByDepartament(int id, CancellationToken cancellationToken)
        {
            return Ok(await _employeeService.GetByDepartamentId(id, cancellationToken));
        }
    }
}
