using RestWithAspNet10_Scaffold.Mail.Settings;

namespace RestWithAspNet10_Scaffold.Configurations
{
    public static class EmailConfig
    {
        public static IServiceCollection ConfigureEmail(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var section = configuration.GetSection("EmailSettings");
            var configs = section.Get<EmailSettings>();

            if(configs == null)
                throw new InvalidOperationException("EmailSettings configuration section is missing or invalid.");

            configs.Username = Environment.GetEnvironmentVariable("EMAIL_USERNAME") ?? configs.Username;
            configs.Password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD") ?? configs.Password;

            services.AddSingleton(configs);

            return services;
        }
    }
}
