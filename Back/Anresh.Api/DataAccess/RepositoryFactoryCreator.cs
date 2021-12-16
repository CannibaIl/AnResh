using Anresh.DataAccess.MsSql;
using Anresh.DataAccess.MsSql.Repositories;
using Anresh.DataAccess.PostgreSql;
using Anresh.Domain.Repositories;
using Microsoft.Extensions.Options;
using System;
using System.Data;

namespace Anresh.Api.DataAccess
{
    public class RepositoryFactoryCreator
    {
        private readonly string _activeDb;
        private readonly IDbConnection _dbConnection;
        public RepositoryFactoryCreator(IOptions<Application.Options> options, IDbConnection dbConnection)
        {
            _activeDb = options.Value.ActiveDb;
            _dbConnection = dbConnection;
        }

        public IRepositoryFactory Create() => _activeDb == "ms" ? new MsSqlRepositoryFactory(_dbConnection) :
                                              _activeDb == "pg" ? new PgSqlRepositoryFactory(_dbConnection) :
                                              throw new NotSupportedException("Unsupported db engine");
    }
}
