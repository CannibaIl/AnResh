using Anresh.Application.Services.User.Contracts;
using Anresh.Domain.DTO;
using System.Threading.Tasks;

namespace Anresh.Application.Services.User.Interfaces
{
    public interface IUserService
    {
        Task EmailConfirmAsync(string token, string password);
        Task<Authenticate.Response> AuthenticateAsync(Authenticate.Request request);
        Task<string> CreateAsync(Create request);
        Task DeleteAsync(int id);
        Task<UserDto> GetCurrentUserAsync();
        Task<UserDto> GetByEmployeeIdAsync(int employeeId);
        Task ChangeRoleAsync(int id, string role);
        Task<string> SendRestorePasswordEmailAsync(string email);
        Task<RestorePassword.Response> RestorePasswordAsync(RestorePassword.Request request);
    }
}
