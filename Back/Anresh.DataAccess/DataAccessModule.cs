using Anresh.DataAccess.Repositories;
using Anresh.Domain.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Data;

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
            services.AddScoped<ISkillRepository, SkillRepository>();
            services.AddScoped<IEmployeeSkillRepisitory, EmployeeSkillRepisitory>();

            return services;
        }
    }
}