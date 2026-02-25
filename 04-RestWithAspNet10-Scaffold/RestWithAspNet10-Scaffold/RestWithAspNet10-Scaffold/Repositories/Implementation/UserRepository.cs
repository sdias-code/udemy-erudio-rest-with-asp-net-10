using RestWithAspNet10_Scaffold.Data;
using RestWithAspNet10_Scaffold.Model;

namespace RestWithAspNet10_Scaffold.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public User? FindByUsername(string username)
        {
            return _context.Users
                .SingleOrDefault(u => u.Username == username);
        }

        public User? FindById(long id)
        {
            return _context.Users.Find(id);
        }

        public bool UsernameExists(string username)
        {
            return _context.Users
                .Any(u => u.Username == username);
        }

        public bool Exists(long id)
        {
            return _context.Users
                .Any(u => u.Id == id);
        }

        public User Create(User user)
        {
            _context.Users.Add(user);
            return user;
        }

        public User? Update(User user)
        {
            var current = _context.Users.Find(user.Id);

            if (current == null)
                return null;

            _context.Entry(current).CurrentValues.SetValues(user);

            return current;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}