using Anresh.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Anresh.Domain.Repositories
{
    public interface IGenericRepository<TEntity,TId> where TEntity : Entity<TId>
    {
        Task<TId> SaveAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TId id);
        Task DeleteMultipleAsync(IEnumerable<TId> ids);
        Task<IEnumerable<TEntity>> FindAllAsync();
        Task<TEntity> FindByIdAsync(int id);
        Task<bool> IsExistsAsync(TId id);
        Task<int> GetTotalRows();
    }
}
