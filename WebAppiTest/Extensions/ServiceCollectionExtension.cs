using Microsoft.EntityFrameworkCore;
using WebAppiTest.Models;
namespace WebAppiTest.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDbContextDefault(this IServiceCollection services, WebApplicationBuilder? builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            services.AddDbContext<MyDataBaseContext>(config =>
            {
                config.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection"));
            });
            return services;
        }

        public static IServiceCollection AddRouteOptionsDefault(this IServiceCollection services)
        {
            services.Configure<RouteOptions>(config =>
            {
                config.LowercaseQueryStrings = true;
                config.LowercaseUrls = true;
            });
            return services;
        }
    }
}