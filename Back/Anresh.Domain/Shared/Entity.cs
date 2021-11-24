using System.Collections.Generic;
using System.Linq;

namespace Anresh.Domain.Shared
{
    public class Entity<TEntity, TId>
    {
        public TId Id { get; set; }
        public IEnumerable<string> GetColumns()
        {
            return typeof(TEntity)
                   .GetProperties()
                   .Where(e => e.Name != "Id")
                   .Select(e => e.Name);
        }
    }
}
