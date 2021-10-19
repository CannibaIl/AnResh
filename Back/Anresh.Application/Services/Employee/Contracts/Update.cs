namespace Anresh.Application.Services.Employee.Contracts
{
    public class Update
    {
        public class Request
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string MidleName { get; set; }
            public int DepartmentId { get; set; }
            public decimal Salary { get; set; }
        }        
    }
}
