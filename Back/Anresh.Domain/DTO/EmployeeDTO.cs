using Anresh.Domain;
using Anresh.Domain.Shared;

namespace Anresh.Domain.DTO
{
    public class EmployeeDTO : Entity<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MidleName { get; set; }
        public decimal Salary { get; set; }
        public Department Department { get; set; }
    }
}
