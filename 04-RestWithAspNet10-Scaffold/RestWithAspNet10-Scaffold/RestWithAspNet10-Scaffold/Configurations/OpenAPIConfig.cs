using Microsoft.OpenApi;

namespace RestWithAspNet10_Scaffold.Configurations
{
    public static class OpenAPIConfig
    {
        private static readonly string AppName = "ASP.NET 10 RESTful API with Swagger, whit Docker and Kubernetes";
        private static readonly string AppDescription = $"A simple ASP.NET 10 RESTful API application, {AppName}";
        public static IServiceCollection AddOpenAPIConfig(this IServiceCollection services)
        {
            services.AddSingleton( new OpenApiInfo
            {
                Title = AppName,
                Version = "v1",
                Description = AppDescription,
                Contact = new OpenApiContact
                {
                    Name = "Your Name",
                    Url = new Uri("https://yourwebsite.com")
                },
                License = new OpenApiLicense
                {
                    Name = "MIT License",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            });

            return services;

        }
    }
}
