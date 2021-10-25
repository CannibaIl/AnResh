using Anresh.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Anresh.Domain.Repositories
{
    public interface IGenericRepository<TEntity,TId> where TEntity : Entity<TId>
    {
        Task<TId> Save(TEntity entity);
        Task Update(TEntity entity);
        Task Delete(TId id, CancellationToken cancellation);
        Task<TEntity> FindById(TId id, CancellationToken cancellationToken);
        Task<List<TEntity>> FindAll(CancellationToken cancellationToken);
    }
}
