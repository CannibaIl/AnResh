using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anresh.Application.Services.Employee.Contracts
{
    public class GetAllWithDepartment : Domain.Employee
    {
        public Domain.Department Department { get; set; }
    }
}
