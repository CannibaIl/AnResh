using System.Collections.Generic;

namespace Anresh.Domain.DTO
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        public int ParentId {  get; set; }
        public string Name { get; set; }
        public int EmployeeCount { get; set; }
        public decimal AverageSalary {  get; set; }
        public List<DepartmentDto> Children { get; set; }
    }
}