using System.Collections.Generic;
using System.Linq;

namespace Anresh.Domain.Shared
{
    public static class EntityExtentions
    {
        public static IEnumerable<string> GetColumns<TId>(this Entity<TId> entity)
        {
            return entity.GetType()
                   .GetProperties()
                   .Where(e => e.Name != "Id")
                   .Select(e => e.Name);
        }
    }
}