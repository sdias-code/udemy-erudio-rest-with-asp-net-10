using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using RestWithAspNet10_Scaffold.Auth.Config;
using RestWithAspNet10_Scaffold.Auth.Contract;
using RestWithAspNet10_Scaffold.DTOs.V1.Account;
using RestWithAspNet10_Scaffold.DTOs.V1.Token;
using RestWithAspNet10_Scaffold.DTOs.V1.User;
using RestWithAspNet10_Scaffold.Model;
using System.Security.Claims;

namespace RestWithAspNet10_Scaffold.Services.Implementations
{
    public class LoginServiceImpl : ILoginService
    {
        private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";

        private readonly IUserAuthService _userAuthService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenService;
        private readonly TokenConfiguration _configurations;

        public LoginServiceImpl(
            IUserAuthService userAuthService,
            IPasswordHasher passwordHasher,
            ITokenGenerator tokenService,
            IOptions<TokenConfiguration> configurations)
        {
            _userAuthService = userAuthService;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _configurations = configurations.Value;
        }

        public TokenDTO? ValidateCredentials(UserDTO userDto)
        {
            var user = _userAuthService
                .FindByUsername(userDto.Username);

            if (user == null) return null;

            if (!_passwordHasher.Verify(userDto.Password, user.PasswordHash))
                return null;

            return GenerateToken(user);
        }

        public TokenDTO? ValidateCredentials(TokenDTO token)
        {
            // Guard against null TokenDTO or missing AccessToken to avoid passing null
            // into GetPrincipalFromExpiredToken which expects a non-null string.
            if (token?.AccessToken == null)
                return null;

            var principal = _tokenService
                .GetPrincipalFromExpiredToken(token.AccessToken);

            // Ensure principal and principal.Identity.Name are present and non-empty.
            // The pattern 'is not string username' both checks for non-null and captures the value.
            if (principal?.Identity?.Name is not string username || string.IsNullOrWhiteSpace(username))
                return null;

            var user = _userAuthService.FindByUsername(username);
            if (user == null ||
                user.RefreshToken != token.RefreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.Now
                )
                return null;

            return GenerateToken(user, principal.Claims);
        }

        public AccountCredentialsDTO Create(AccountCredentialsDTO dto)
        {
            var user = _userAuthService
                .Create(dto);

            return new AccountCredentialsDTO
            {
                Username = user.UserName,
                Fullname = user.FullName,
                Password = "************"
            };
        }

        public bool RevokeToken(string username)
        {
            return _userAuthService
                .RevokeToken(username);
        }

        private TokenDTO GenerateToken(User user,
            IEnumerable<Claim>? existingClaims = null)
        {
            var claims = existingClaims?.ToList() ??                
                [
                    new Claim(JwtRegisteredClaimNames.Jti,
                        Guid.NewGuid().ToString("N")),

                    new Claim(JwtRegisteredClaimNames.UniqueName,
                        user.UserName),
                ];

            var accessToken = _tokenService
                .GenerateAccessToken(claims);

            var refreshToken = _tokenService
                .GenerateRefreshToken();

            user.RefreshToken = refreshToken;

            user.RefreshTokenExpiryTime = DateTime.UtcNow
                .AddDays(_configurations.DaysToExpiry);

            _userAuthService.UpdateRefreshToken(user);

            var createdDate = DateTime.UtcNow;

            var expirationDate = createdDate
                .AddMinutes(_configurations.Minutes);

            return new TokenDTO
            {
                Authenticated = true,
                Created = createdDate.ToString(DATE_FORMAT),
                Expiration = expirationDate
                    .ToString(DATE_FORMAT),

                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
