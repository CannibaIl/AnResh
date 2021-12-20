using System.Collections.Generic;

namespace Anresh.Domain.DTO
{
    public class EmployeesFilter : PageParams
    {
        public int? DepartmentID { get; set; }
        public string FirstName { get; set; }
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public List<int> ListSkillsId { get; set; }
    }
}
