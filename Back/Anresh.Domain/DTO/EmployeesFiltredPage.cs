using Anresh.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anresh.Domain.DTO
{
    public class EmployeesFiltredPage
    {
        public IEnumerable<EmployeeDto> Employees {  get; set; }
        public int TakeCount { get; set; }
        public int Total {  get; set; }
    }
}
