using System.Threading.Tasks;

namespace Anresh.Domain.Repositories
{
    public interface IUserRepository : IGenericRepository<User, int>
    {
        Task<User> FindByEmailAsync(string name);
        int? GetUserIdByToken(string token);
        User GetCurrentUser();
        Task<Domain.DTO.UserDto> GetByEmployeeIdAsync(int employeeId);
        Task ChangeRoleAsync(int id, string role);
        Task RestorePasswordAsync(int id, byte[] hash, byte[] salt);
    }
}
