using Microsoft.OpenApi;

namespace RestWithAspNet10_Scaffold.Configurations
{
    public static class SwaggerConfig
    {
        private static readonly string AppName = "ASP.NET 10 RESTful API with Swagger, whit Docker and Kubernetes";
        private static readonly string AppDescription = $"A simple ASP.NET 10 RESTful API application, {AppName}";

        public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = AppName,
                    Version = "v1",
                    Description = AppDescription,
                    Contact = new OpenApiContact
                    {
                        Name = "Silvio Dias",
                        Email = "silviodias.ms@gmail.com",
                        Url = new Uri("https://www.linkedin.com/in/sdias2026")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });

                c.CustomSchemaIds(type => type.FullName);
            });
            return services;
        }

        public static IApplicationBuilder UseSwaggerConfig(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                c.RoutePrefix = "swagger";
                c.DocumentTitle = AppName;
            });
            return app;
        }


    }
}
