using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RestWithAspNet10_Scaffold.Auth.Config
{
    public static class JwtConfigurationExtension
    {
        public static IServiceCollection AddJwtConfiguration(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var tokenConfig = configuration
                .GetSection("TokenConfiguration")
                .Get<TokenConfiguration>();

            if (tokenConfig == null)
                throw new InvalidOperationException("TokenConfiguration not found.");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,

                            ValidIssuer = tokenConfig.Issuer,
                            ValidAudience = tokenConfig.Audience,

                            IssuerSigningKey =
                                new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(tokenConfig.Secret))
                        };
                });

            services.AddAuthorization();

            return services;
        }
    }
}