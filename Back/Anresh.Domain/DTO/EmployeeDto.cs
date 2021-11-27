using System.Collections.Generic;

namespace Anresh.Domain.DTO
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public decimal Salary { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public List<Skill> Skills {  get; set; }
    }
}
