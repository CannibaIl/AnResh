using System.Threading.Tasks;

namespace Anresh.Domain.Repositories
{
    public interface IUserRepository : IGenericRepository<User, int>
    {
        Task<User> FindByEmailAsync(string name);
        Task<bool> IsAdmin(int id);
        Task<DTO.UserDto> FindByEmployeeIdAsync(int employeeId);
        Task<Domain.DTO.UserDto> FindByIdWithEmployeeDataAsync(int id);
        Task ChangeRoleAsync(int id, string role);
        Task RestorePasswordAsync(int id, byte[] hash, byte[] salt);
    }
}
