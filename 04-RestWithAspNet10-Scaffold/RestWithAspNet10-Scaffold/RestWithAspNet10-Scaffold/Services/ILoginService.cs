using RestWithAspNet10_Scaffold.DTOs.V1.Account;
using RestWithAspNet10_Scaffold.DTOs.V1.Token;
using RestWithAspNet10_Scaffold.DTOs.V1.User;

namespace RestWithAspNet10_Scaffold.Services
{
    public interface ILoginService
    {
        TokenDTO? ValidateCredentials(UserDTO user);
        TokenDTO? ValidateCredentials(TokenDTO token);
        bool RevokeToken(string username);
        AccountCredentialsDTO Create(AccountCredentialsDTO user);
    }
}
