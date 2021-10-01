using Anresh.Application.Services.Department.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Anresh.Controllers
{
    [Route("api/departament")]
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
    }
}
