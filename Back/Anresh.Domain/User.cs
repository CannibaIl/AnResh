using Anresh.Domain.Shared;

namespace Anresh.Domain
{
    [Table("Users")]
    public sealed class User : Entity<int>
    {
        public int EmployeeId {  get; set; }
        public string Email { get; set; }
        public bool HasEmailConfirm { get; set; }
        public byte[] HashPassword { get; set; }
        public byte[] SaltPassword { get; set; }
        public string Role {  get; set; }
    }
}
