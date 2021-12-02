using Anresh.Application.Services.Auth.Implementations;
using Anresh.Application.Services.Auth.Interfaces;
using Anresh.Application.Services.Department.Implementations;
using Anresh.Application.Services.Department.Interfaces;
using Anresh.Application.Services.Employee.Implementations;
using Anresh.Application.Services.Employee.Interfaces;
using Anresh.Application.Services.File.Implementations;
using Anresh.Application.Services.File.Interfaces;
using Anresh.Application.Services.Skill.Implementations;
using Anresh.Application.Services.Skill.Interfaces;
using Anresh.Application.Services.User.Implementations;
using Anresh.Application.Services.User.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Anresh.Application
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplicationModule(this IServiceCollection services)
        {
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISkillService, SkillService>();
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
