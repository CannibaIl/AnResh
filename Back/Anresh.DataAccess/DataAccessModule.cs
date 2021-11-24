using Anresh.Domain.Repositories;
using Anresh.DataAccess.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using Microsoft.Extensions.Options;

namespace Anresh.DataAccess
{
    public static class DataAccessModule
    {
        public static IServiceCollection AddDataAccessModule(this IServiceCollection services)
        {
            services.AddScoped<IDbConnection>((sp) =>
               new SqlConnection(sp.GetService<IOptions<Application.Options>>().Value.ConnectionStrings.SqlServerDb)
            );

            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}