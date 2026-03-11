using Scalar.AspNetCore;

namespace RestWithAspNet10_Scaffold.Configurations
{
    public static class ScalarConfig
    {
        private static readonly string AppName = "ASP.NET 10 RESTful API with Swagger, whit Docker and Kubernetes";
        public static WebApplication UseScalarConfig(
            this WebApplication app)
        {

            app.MapScalarApiReference("/scalar", options =>

            {
                options
                .WithTitle(AppName)
                .WithOpenApiRoutePattern("/swagger/v1/swagger.json");

            });

            return app;

        }
        

    }
}
