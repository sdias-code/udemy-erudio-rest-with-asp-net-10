namespace RestWithAspNet10_Scaffold.Configurations
{
    public static class RouteConfig
    {
        public static IServiceCollection AddRouteConfiguration(this IServiceCollection services)
        {
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
                options.AppendTrailingSlash = false;

            });

            return services;
        }
    }
}
