using System.Collections.Generic;

namespace Anresh.Domain.DTO
{
    public class DepartmentSimpleDto
    {
        public int Id { get; set; }
        public int ParentId {  get; set; }
        public string Name { get; set; }
        public List<DepartmentDto> Children { get; set; }
    }
}
