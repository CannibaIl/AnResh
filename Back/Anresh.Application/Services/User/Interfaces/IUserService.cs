using Anresh.Application.Services.User.Contracts;
using Anresh.Domain.DTO;
using System.Threading.Tasks;

namespace Anresh.Application.Services.User.Interfaces
{
    public interface IUserService
    {
        Task<Register.Response> RegisterAsync(Register.Request request);
        Task<bool> EmailConfirmAsync(string token);
        Task<Authenticate.Response> AuthenticateAsync(Authenticate.Request request);
        Task<Create.Response> CreateAsync(Create.Request request);
        Task DeleteAsync(int id);
        Task<UserDto> GetCurrentUserAsync();
        Task<UserDto> GetByEmployeeIdAsync(int employeeId);
        Task ChangeRoleAsync(int id, string role);
        Task<string> SendRestorePasswordEmailAsync(string email);
        Task<RestorePassword.Response> RestorePasswordAsync(RestorePassword.Request request);
    }
}
