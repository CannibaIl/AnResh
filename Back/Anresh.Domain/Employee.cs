using Anresh.Domain.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Anresh.Domain
{
    public sealed class Employee : Entity<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public decimal Salary { get; set; }
        public int DepartmentId { get; set; }
    }
}
