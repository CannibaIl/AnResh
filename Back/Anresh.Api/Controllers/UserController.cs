using Anresh.Api.Controllers.Requests.User;
using Anresh.Application.Services.User.Contracts;
using Anresh.Application.Services.User.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Anresh.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserRequest request)
        {
            var response = await _userService.RegisterAsync(new Register.Request()
            {
                Email = request.Email,
                Password = request.Password,
                EmployeeId = request.EmployeeId
            });
            return Ok(response);
        }

        [HttpGet("confirm")]
        [AllowAnonymous]
        public async Task<IActionResult> EmailConfirmAsync(string token)
        {
            var isSuccessful = await _userService.EmailConfirmAsync(token);
            if (isSuccessful)
            {
                return Ok("Email confirm");
            }
            return BadRequest("Invalid or old token");
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> AuthenticateAsync([FromBody] UserRequest request)
        {
            var response = await _userService.AuthenticateAsync(new Authenticate.Request()
            {
                UserName = request.Email,
                Password = request.Password
            });
            return Ok(response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateUserRequest request)
        {
            var response = await _userService.CreateAsync(new Create.Request()
            {
                EmployeeId = request.EmployeeId,
                Email = request.Email,
                Role = request.Role,
                Password = request.Password
            });
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentUserAsync()
        {
            return Ok(await _userService.GetCurrentUserAsync());
        }

        [HttpGet("employee/{id}")]
        public async Task<IActionResult> GetByEmployeeIdAsync([FromRoute] int id)
        {
            return Ok(await _userService.GetByEmployeeIdAsync(id));
        }

        [HttpPut("role")]
        public async Task<IActionResult> UpdateAsync( [FromBody] ChangeRoleRequest request)
        {
            await _userService.ChangeRoleAsync(request.Id, request.Role);
            return Ok();
        }

        [HttpPost("sendRestorePasswordEmail/{email}")]
        [AllowAnonymous]
        public async Task<IActionResult> SendRestorePasswordEmailAsync([FromRoute] string email)
        {
            var result = await _userService.SendRestorePasswordEmailAsync(email);
            return Ok(result);
        }   

        [HttpPost("RestorePassword")]
        [AllowAnonymous]
        public async Task<IActionResult> SendRestorePasswordEmailAsync([FromBody] RestorePasswordRequest request)
        {
            var result = await _userService.RestorePasswordAsync(new RestorePassword.Request()
            {
                Token = request.Token,
                Password = request.Password
            });
            return Ok(result);
        }
    }
}
