using Anresh.Domain.Shared;

namespace Anresh.Domain
{
    [Table("EmployeesSkills")]
    public class EmployeeSkill : Entity<EmployeeSkill, int>
    {
        public int EmployeeId { get; set; }
        public int SkillId { get; set; }
    }
}
