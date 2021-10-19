using Anresh.Domain.Shared;
using System.Collections.Generic;

namespace Anresh.Domain.DTO
{
    public class DepartmentDTO : Entity<int>
    {
        public string Name { get; set; }
        public List<Employee> Employees { get; set; }
    }
}
