using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Data;

namespace Anresh.Api.DataAccess
{
    public class DbConnectionFactory
    {
        private readonly Application.Options _options;
        public DbConnectionFactory(IServiceProvider sp)
        {
            _options = sp.GetService<IOptions<Application.Options>>().Value;
        }

        public IDbConnection Create() => _options.ActiveDb == "ms" ? new SqlConnection(_options.ConnectionStrings.SqlServerDb) :
                                         _options.ActiveDb == "pg" ? new NpgsqlConnection(_options.ConnectionStrings.PostgreServerDb) :
                                         throw new NotSupportedException("Unsupported db engine");
    }
}
