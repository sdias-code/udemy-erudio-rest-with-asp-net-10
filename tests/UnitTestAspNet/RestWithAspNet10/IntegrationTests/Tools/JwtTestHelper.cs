using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RestWithAspNet10_Scaffold.Auth.Config;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestWithAspNet10.IntegrationTests.Tools
{
    public static class JwtTestHelper
    {
        public static string GenerateToken(IConfiguration configuration)
        {
            var tokenConfig = configuration
                .GetSection("TokenConfiguration")
                .Get<TokenConfiguration>();

            if (tokenConfig == null)
                throw new InvalidOperationException("TokenConfiguration not found.");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(tokenConfig.Secret));

            var credentials = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "testuser"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var token = new JwtSecurityToken(
                issuer: tokenConfig.Issuer,
                audience: tokenConfig.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}