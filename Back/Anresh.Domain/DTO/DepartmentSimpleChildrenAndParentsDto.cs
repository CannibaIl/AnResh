using System.Collections.Generic;

namespace Anresh.Domain.DTO
{
    public class DepartmentSimpleChildrenAndParentsDto
    {
        public IEnumerable<DepartmentSimpleDto> Children { get; set; }
        public IEnumerable<DepartmentSimpleDto> Parents { get; set; }

        public DepartmentSimpleChildrenAndParentsDto(IEnumerable<DepartmentSimpleDto> children, IEnumerable<DepartmentSimpleDto> parents)
        {
            Children = children;
            Parents = parents;
        }
    }
}
