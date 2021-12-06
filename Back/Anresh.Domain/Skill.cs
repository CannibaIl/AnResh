using Anresh.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anresh.Domain
{
    [Table("Skills")]
    public class Skill : Entity<int>
    {
        public string Name { get; set; }
    }
}
