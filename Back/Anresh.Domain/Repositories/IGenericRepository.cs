using Anresh.Domain.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Anresh.Domain.Repositories
{
    public interface IGenericRepository<TEntity,TId> where TEntity : Entity<TEntity, TId>
    {
        Task<TId> SaveAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TId id);
        Task DeleteMultipleAsync(IEnumerable<TId> ids);
        Task<IEnumerable<TEntity>> FindAllAsync();
        Task<TEntity> FindByIdAsync(int id);
        Task<bool> IsExistsAsync(TId id);
    }
}
