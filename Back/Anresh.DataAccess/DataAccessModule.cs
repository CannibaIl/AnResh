using Anresh.Domain.Repositories;
using Anresh.DataAccess.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace Anresh.DataAccess
{
    public static class DataAccessModule
    {
        private const string ConnectionStringName = "SqlServerDb";
        public static IServiceCollection AddDataAccessModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDbConnection>((sp) =>
               new SqlConnection(configuration.GetConnectionString(ConnectionStringName))
            );

            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();

            return services;
        }
    }
}
