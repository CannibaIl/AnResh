using Anresh.Domain.Shared;

namespace Anresh.Domain
{
    public sealed class Department : Entity<int>
    {
        public string Name { get; set; }
    }
}