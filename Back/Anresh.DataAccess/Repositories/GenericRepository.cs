using Anresh.Domain.Repositories;
using Anresh.Domain.Shared;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
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
        private static IEnumerable<string> GetColumns()
        {
            return typeof(TEntity)
                    .GetProperties()
                    .Where(e => e.Name != "Id" && !e.PropertyType.GetTypeInfo().IsGenericType)
                    .Select(e => e.Name);
        }
        public async Task<TId> Save(TEntity entity)
        {
            var columns = GetColumns();
            var stringOfColumns = string.Join(", ", columns);
            var stringOfParameters = string.Join(", ", columns.Select(e => "@" + e));
            var query = await DbConnection.QueryAsync<TId>($"insert into {TableName} ({stringOfColumns}) OUTPUT INSERTED.* values ({stringOfParameters})", entity);

            var id = query.First();
            return id;
        }

        public async Task Update(TEntity entity)
        {
            var columns = GetColumns();
            var stringOfColumns = string.Join(", ", columns.Select(e => $"{e} = @{e}"));
            await DbConnection.ExecuteAsync($"UPDATE {TableName} SET {stringOfColumns} WHERE Id = @Id", entity);
        }
        public async Task Delete(TId id, CancellationToken cancellation)
        {
            await DbConnection.ExecuteAsync($"DELETE FROM {TableName} WHERE Id = {id}", cancellation);
        }
        public async Task<TEntity> FindById(TId id, CancellationToken cancellationToken)
        {
            return await DbConnection.QueryFirstOrDefaultAsync<TEntity>($"SELECT * FROM {TableName} WHERE Id = {id}", cancellationToken);
        }
        public async Task<List<TEntity>> FindAll(CancellationToken cancellationToken)
        {
            var data = await DbConnection.QueryAsync<TEntity>($"SELECT * FROM {TableName}", cancellationToken);
            return data.ToList();
        }
    }
}
