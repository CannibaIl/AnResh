using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Anresh.Api
{
    public static class RoutingModule
    {
        public static IApplicationBuilder AddRoutingModule(this IApplicationBuilder app)
        {
            return app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/employees/{id:int}", async context =>
                {
                    await context.Response.WriteAsync(await System.IO.File.ReadAllTextAsync(@"wwwroot/employees.html"));
                });
                endpoints.MapGet("/employees", async context =>
                {
                    await context.Response.WriteAsync(await System.IO.File.ReadAllTextAsync(@"wwwroot/employees.html"));
                });
                endpoints.MapGet("/departments", async context =>
                {
                    await context.Response.WriteAsync(await System.IO.File.ReadAllTextAsync(@"wwwroot/departments.html"));
                });
                endpoints.MapGet("/file", async context =>
                {
                    await context.Response.WriteAsync(await System.IO.File.ReadAllTextAsync(@"wwwroot/file.html"));
                });
            });
        }
    }
}
