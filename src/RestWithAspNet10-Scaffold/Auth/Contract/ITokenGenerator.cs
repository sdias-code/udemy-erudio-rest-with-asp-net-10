using RestWithAspNet10_Scaffold.Model;
using System.Security.Claims;

namespace RestWithAspNet10_Scaffold.Auth.Contract
{
    public interface ITokenGenerator
    {
            string GenerateAccessToken(IEnumerable<Claim> claims);
            string GenerateRefreshToken();
            ClaimsPrincipal GetPrincipalFromExpiredToken(string token);        
    }
}
