using Anresh.Domain.Repositories;
using Anresh.Domain.Shared;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
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

        //это можно вынести как extention method для Entity
        private static IEnumerable<string> GetColumns()
        {
            return typeof(TEntity)
                    .GetProperties()
                    .Where(e => e.Name != "Id")
                    .Select(e => e.Name);
        }

        public async Task<TId> Save(TEntity entity)
        {
            var columns = GetColumns();
            var columnNames = string.Join(", ", columns);
            var parameterNames = string.Join(", ", columns.Select(e => "@" + e));
            var query = await DbConnection.QueryAsync<TId>($"insert into { TableName } ({columnNames}) OUTPUT INSERTED.* values ({parameterNames})", entity);

            return query.First();
        }

        public async Task Update(TEntity entity)
        {
            var columns = GetColumns();
            var columnNames = string.Join(", ", columns.Select(e => $"{e} = @{e}"));
            await DbConnection.ExecuteAsync($"UPDATE { TableName } SET {columnNames} WHERE Id = {entity.Id}", entity);
        }

        public async Task Delete(TId id)
        {
            var sql = $"DELETE FROM {TableName} WHERE Id = @id";
            await DbConnection.ExecuteAsync(sql, new { id });
        }

        public async Task DeleteMultiple(IEnumerable<TId> listId)
        {
            var stringOfIds = string.Join(", ", listId);
            var sql = $"DELETE FROM { TableName } WHERE Id IN ({stringOfIds})";
            await DbConnection.ExecuteAsync(sql);
        }

        public async Task<IEnumerable<TEntity>> FindAll() {
            var sql = $"SELECT * FROM {TableName}";
            return await DbConnection.QueryAsync<TEntity>(sql);
        }

        public async Task<TEntity> FindById(int id)
        {
            var sql = $@"SELECT * FROM {TableName} WHERE Id = @id";
            return await DbConnection.QuerySingleOrDefaultAsync<TEntity>(sql, new { id });
        }

        public async Task<bool> IsExists(TId id)
        {
            return await DbConnection.ExecuteScalarAsync<bool>($"SELECT COUNT(1) FROM {TableName} WHERE Id = {id}");
        }
    }
}
