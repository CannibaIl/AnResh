using Anresh.Domain.Shared;

namespace Anresh.Domain
{
    [Table("EmployeesSkills")]
    public sealed class EmployeeSkill : Entity<int>
    {
        public int EmployeeId { get; set; }
        public int SkillId { get; set; }
    }
}
