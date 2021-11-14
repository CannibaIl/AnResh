using Anresh.Domain.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Anresh.Domain.Repositories
{
    public interface IGenericRepository<TEntity,TId> where TEntity : Entity<TId>
    {
        Task<TId> Save(TEntity entity);
        Task Update(TEntity entity);
        Task Delete(TId id);
        Task DeleteMultiple(IEnumerable<TId> ids);
        Task<IEnumerable<TEntity>> FindAll();
        Task<TEntity> FindById(int id);
        Task<bool> IsExists(TId id);
    }
}
