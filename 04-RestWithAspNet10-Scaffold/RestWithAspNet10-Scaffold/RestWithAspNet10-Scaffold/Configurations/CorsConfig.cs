using System.Configuration;

namespace RestWithAspNet10_Scaffold.Configurations
{
    public static class CorsConfig
    {    

        public static void AddCorsConfiguration(
            this IServiceCollection services, 
            IConfiguration configuration)
        {

            var origins = configuration
               .GetSection("Cors:Origins")
               .Get<string[]>() ?? Array.Empty<string>();

            services.AddCors(options =>
             {
                 options.AddPolicy("LocalPolicy", policy =>
                 {
                     policy
                     .WithOrigins("http://localhost:3000", "http://localhost:3001")
                     .AllowAnyHeader()
                     .AllowAnyMethod();
                     //.AllowCredentials();
                 });

                 options.AddPolicy("DefaultPolicy", policy =>
                 {
                     policy
                     .WithOrigins(origins)
                     .AllowAnyHeader()
                     .AllowAnyMethod();
                     //.AllowCredentials();
                 });
             });
        }

        
    }
}
