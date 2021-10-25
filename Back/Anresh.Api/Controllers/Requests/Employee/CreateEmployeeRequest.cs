using System.ComponentModel.DataAnnotations;

namespace Anresh.Api.Controllers.Requests.Employee
{
    public class CreateEmployeeRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MidleName { get; set; }
        public int DepartmentId { get; set; }
        public decimal Salary { get; set; }
    }
}
