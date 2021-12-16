using Anresh.Api.DataAccess;
using Anresh.Application;
using Anresh.DataAccess.PostgreSql;
using Anresh.Domain.Repositories;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Anresh.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Application.Options>(Configuration.GetSection("MyOptions"));

            //var sp = services.BuildServiceProvider();
            //var options = sp.GetService<IOptions<Application.Options>>().Value;


            services.AddAuthentication("OAuth")
                .AddJwtBearer("OAuth", config =>
                {
                    config.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Configuration["ApiUri"],
                        ValidAudience = Configuration["ApiUri"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token:Key"]))
                    };
                });

            services.AddCors(
                options =>
                {
                    options.AddPolicy(name: MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins(Configuration["FrontUri"])
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
                });

            services.AddControllers().AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<Startup>();
            });

            services.AddSwaggerModule();
            
            services.AddScoped<DbConnectionFactory>()
                    .AddScoped((sp) => sp.GetService<DbConnectionFactory>().Create());

            services.AddScoped<RepositoryFactoryCreator>()
                    .AddScoped((sp) => sp.GetService<RepositoryFactoryCreator>().Create());

            #region register all repos

            services.AddScoped(sp => sp.GetService<IRepositoryFactory>().CreateDepartmentRepository());
            services.AddScoped(sp => sp.GetService<IRepositoryFactory>().CreateEmployeeRepository());
            services.AddScoped(sp => sp.GetService<IRepositoryFactory>().CreateEmployeeSkillRepository());
            services.AddScoped(sp => sp.GetService<IRepositoryFactory>().CreateSkillRepository());
            services.AddScoped(sp => sp.GetService<IRepositoryFactory>().CreateUserRepository());
            #endregion


            services.AddApplicationModule();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Anresh v1"));
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
