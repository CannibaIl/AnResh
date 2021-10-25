using Anresh.Domain.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Anresh.Domain
{
    public class Employee : Entity<int>
    {
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public string MidleName { get; set; }     
        public decimal Salary {  get; set; }
        public int DepartmentId { get; set; }
    }
}
