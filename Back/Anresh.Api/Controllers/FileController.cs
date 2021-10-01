using Anresh.Application.Services.File.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Anresh.Controllers
{
    [Route("api/file")]
    [ApiController]
    public partial class FileController : Controller
    {
        private readonly IFileService _fileService;
        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet]

        public IActionResult Load()
        {
            return Ok(_fileService.Load());
        }

        [HttpPost]
        public IActionResult Save([FromForm] string text)
        {
            _fileService.Save(text);
            return Ok();
        }
    }
}
