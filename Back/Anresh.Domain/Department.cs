using Anresh.Domain.Shared;
using Anresh.Domain.DTO;

namespace Anresh.Domain
{
    [Table("Departments")]
    public sealed class Department : Entity<int>
    {
        public Department()
        {
        }
        public Department(DepartmentDto dto)
        {
            Name = dto.Name;
        }
        public string Name { get; set; }
    }
}