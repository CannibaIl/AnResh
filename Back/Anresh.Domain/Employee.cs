using Anresh.Domain.Shared;


namespace Anresh.Domain
{
    public class Employee : Entity<int>
    {
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public string MidleName { get; set; }
        public int DepartmentId {  get; set; }
        public decimal Salary {  get; set; }
    }
}
