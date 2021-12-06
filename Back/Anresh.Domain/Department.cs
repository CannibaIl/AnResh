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
            Id = dto.Id;
            Name = dto.Name;
            ParentId = dto.ParentId;
        }
        public string Name { get; set; }
        public int? ParentId {  get; set; }
    }
}