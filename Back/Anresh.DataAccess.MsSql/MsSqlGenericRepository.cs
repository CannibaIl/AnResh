using Anresh.Domain.Repositories;
using Anresh.Domain.Shared;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anresh.DataAccess.MsSql.Repositories
{
    public abstract class MsSqlGenericRepository<TEntity, TId> : IGenericRepository<TEntity, TId> where TEntity : Entity<TId>
    {
        protected string TableName;
        protected readonly IDbConnection DbConnection;
        private protected MsSqlGenericRepository(IDbConnection dbConnection)
        {
            DbConnection = dbConnection;
            var tableAttribute = (TableAttribute)typeof(TEntity).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault();
            if (tableAttribute is null)
            {
                throw new Exception("table attribute not found");
            }
            TableName = tableAttribute.Table;
        }

        public async Task<TId> SaveAsync(TEntity entity)
        {
            var columnNames = string.Join(", ", entity.GetColumns());
            var parameterNames = string.Join(", ", entity.GetColumns().Select(e => "@" + e));
            var sql = $"INSERT INTO { TableName } ({columnNames}) OUTPUT INSERTED.* VALUES ({parameterNames})";
            var query = await DbConnection.QueryAsync<TId>(sql, entity);

            return query.First();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            var columnNames = string.Join(", ", entity.GetColumns().Select(e => $"{e} = @{e}"));
            var sql = $"UPDATE { TableName } SET {columnNames} WHERE Id = {entity.Id}";
            await DbConnection.ExecuteAsync(sql, entity);
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
            var sql = $"SELECT COUNT(1) FROM {TableName} WHERE Id = {id}";
            return await DbConnection.ExecuteScalarAsync<bool>(sql);
        }

        public async Task<int> GetTotalRows()
        {
            var sql = $@"SELECT count(t.Id) FROM {TableName} t";
            return await DbConnection.QuerySingleOrDefaultAsync<int>(sql);
        }
    }
}