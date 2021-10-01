using Anresh.Domain.Repositories;
using Anresh.Domain.Shared;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Anresh.DataAccess.Repositories
{
    public class GenericRepository<TEntity, TId> : IGenericRepository<TEntity, TId> where TEntity : Entity<TId>
    {
        protected readonly IDbConnection DbConnection;
        protected string TableName;
        private protected GenericRepository(IDbConnection dbConnection, string departments)
        {
            DbConnection = dbConnection;
            TableName = departments;
        }
        public async Task<TEntity> FindById(TId id, CancellationToken cancellationToken)
        {
            return await DbConnection.QueryFirstOrDefaultAsync<TEntity>($"SELECT * FROM {TableName} WHERE Id = {id}", cancellationToken);
        }
        public async Task<TEntity> FindFirstOrDefaultWhere(string colum, string value, CancellationToken cancellationToken)
        {
            return await DbConnection.QueryFirstOrDefaultAsync<TEntity>($"SELECT * FROM {TableName} WHERE {colum} = {value}", cancellationToken);
        }
        public async Task<List<TEntity>> FindWhere(string colum, string value, CancellationToken cancellationToken)
        {
            var data = await DbConnection.QueryAsync<TEntity>($"SELECT * FROM {TableName} WHERE {colum} = {value}", cancellationToken);
            return data.ToList();
        }
        public async Task<List<TEntity>> FindAll(CancellationToken cancellationToken)
        {
            var data = await DbConnection.QueryAsync<TEntity>($"SELECT * FROM {TableName}", cancellationToken);
            return data.ToList();
        }
    }
}
