using Anresh.Domain.DTO;
using Anresh.Domain.Shared;


namespace Anresh.Domain
{
    [Table("Employees")]
    public sealed class Employee : Entity<int>
    {
        public Employee()
        {
        }
        public Employee(EmployeeDto dto)
        {
            Id = dto.Id;
            FirstName = dto.FirstName;
            LastName = dto.LastName;
            MiddleName = dto.MiddleName;
            Salary = dto.Salary;
            DepartmentId = dto.DepartmentId;

        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public decimal Salary { get; set; }
        public int DepartmentId { get; set; }
    }
}
