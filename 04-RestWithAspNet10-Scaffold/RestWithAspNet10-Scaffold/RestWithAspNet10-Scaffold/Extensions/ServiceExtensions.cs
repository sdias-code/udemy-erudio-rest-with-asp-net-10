using Microsoft.EntityFrameworkCore;
using RestWithAspNet10_Scaffold.Data;

namespace RestWithAspNet10_Scaffold.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureSqlServer(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));
        }
    }
}
