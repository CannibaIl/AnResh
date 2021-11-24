using Anresh.Domain.Shared;


namespace Anresh.Domain
{
    [Table("Employees")]
    public sealed class Employee : Entity<Employee, int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public decimal Salary { get; set; }
        public int DepartmentId { get; set; }
    }
}
