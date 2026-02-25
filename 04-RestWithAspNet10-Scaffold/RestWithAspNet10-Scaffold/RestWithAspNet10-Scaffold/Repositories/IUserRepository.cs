using RestWithAspNet10_Scaffold.Model;

namespace RestWithAspNet10_Scaffold.Repositories
{
    public interface IUserRepository
    {
        User? FindByUsername(string username);
        User? FindById(long id);

        User Create(User user);
        User? Update(User user);

        bool Exists(long id);
        bool UsernameExists(string username);

        void Save();
    }
}
