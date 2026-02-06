namespace RestWithAspNet10_Scaffold.Configurations
{
    public static class CorsConfig
    {
        public static void AddCorsConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
             {
                 options.AddPolicy("LocalPolicy", policy =>
                 {
                     policy
                     .WithOrigins("http://localhost:3000", "http://localhost:3001")
                      //.AllowAnyOrigin()
                     .AllowAnyHeader()
                     .AllowAnyMethod()
                     .AllowCredentials();
                 });
             });
        }

        public static IApplicationBuilder UseCorsConfiguration(this IApplicationBuilder app)
        {
            app.UseCors("LocalPolicy");            

            return app;

        }
    }
}
