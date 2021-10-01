using Anresh.Domain.Shared;
using System;

namespace Anresh.Domain
{
    public class Department : Entity<int>
    {
        public string Name {  get; set; }
    }
}
