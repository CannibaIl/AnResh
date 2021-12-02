using Anresh.Domain.DTO;
using System.Threading.Tasks;

namespace Anresh.Application.Services.Auth.Interfaces
{
    public interface IAuthService
    {
        string CreateToken(int id);
        int? GetCurrentUserId();
        Task<UserDto> GetCurrentUserAsync();
        int? GetUserIdByToken(string token);
    }
}
