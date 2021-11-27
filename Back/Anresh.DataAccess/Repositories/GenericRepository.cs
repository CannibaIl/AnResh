using Anresh.Domain.Repositories;
using Anresh.Domain.Shared;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Anresh.DataAccess.Repositories
{
    public class GenericRepository<TEntity, TId> : IGenericRepository<TEntity, TId> where TEntity : Entity<TEntity, TId>
    {
        protected readonly IDbConnection DbConnection;
        protected string TableName;
        private protected GenericRepository(IDbConnection dbConnection)
        {
            DbConnection = dbConnection;
            TableName = ((TableAttribute)typeof(TEntity).GetCustomAttributes(typeof(TableAttribute), false)[0]).Table;
        }

        public async Task<TId> SaveAsync(TEntity entity)
        {
            var columnNames = string.Join(", ", entity.GetColumns());
            var parameterNames = string.Join(", ", entity.GetColumns().Select(e => "@" + e));
            var sql = $"insert into { TableName } ({columnNames}) OUTPUT INSERTED.* values ({parameterNames})";
            var query = await DbConnection.QueryAsync<TId>(sql, entity);

            return query.First();
        }

        public async Task SaveMultipleAsync(string values)
        {
            var entity = new Entity<TEntity, TId>();

            var columnNames = string.Join(", ", entity.GetColumns());
            var sql = $"insert into { TableName } ({columnNames}) values {values}";
            await DbConnection.QueryAsync<TId>(sql);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            var columnNames = string.Join(", ", entity.GetColumns().Select(e => $"{e} = @{e}"));
            await DbConnection.ExecuteAsync($"UPDATE { TableName } SET {columnNames} WHERE Id = {entity.Id}", entity);
        }

        public async Task DeleteAsync(TId id)
        {
            var sql = $"DELETE FROM {TableName} WHERE Id = @id";
            await DbConnection.ExecuteAsync(sql, new { id });
        }

        public async Task DeleteMultipleAsync(IEnumerable<TId> listId)
        {
            var stringOfIds = string.Join(", ", listId);
            var sql = $"DELETE FROM { TableName } WHERE Id IN ({stringOfIds})";
            await DbConnection.ExecuteAsync(sql);
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync()
        {
            var sql = $"SELECT * FROM {TableName}";
            return await DbConnection.QueryAsync<TEntity>(sql);
        }

        public async Task<TEntity> FindByIdAsync(int id)
        {
            var sql = $@"SELECT * FROM {TableName} WHERE Id = @id";
            return await DbConnection.QuerySingleOrDefaultAsync<TEntity>(sql, new { id });
        }

        public async Task<bool> IsExistsAsync(TId id)
        {
            return await DbConnection.ExecuteScalarAsync<bool>($"SELECT COUNT(1) FROM {TableName} WHERE Id = {id}");
        }
    }
}
