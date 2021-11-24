namespace Anresh.Application.Services.Employee.Contracts
{
    public class Update
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int DepartmentId { get; set; }
        public decimal Salary { get; set; }
    }
}
