using Anresh.Domain.Shared;

namespace Anresh.Domain
{
    [Table("Departments")]
    public sealed class Department : Entity<Department,int>
    {
        public string Name { get; set; }
    }
}