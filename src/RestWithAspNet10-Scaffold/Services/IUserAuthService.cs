using RestWithAspNet10_Scaffold.DTOs.V1.Account;
using RestWithAspNet10_Scaffold.DTOs.V1.User;
using RestWithAspNet10_Scaffold.Model;

namespace RestWithAspNet10_Scaffold.Services
{
    public interface IUserAuthService
    {
        User? FindByUsername(string username);
        User Create(AccountCredentialsDTO dto);
        bool RevokeToken(string username);
        User UpdateProfile(UpdateUserDTO dto);        
        void UpdateRefreshToken(User user);
    }
}
