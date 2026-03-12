using Microsoft.AspNetCore.Identity;
using RestWithAspNet10_Scaffold.Auth.Contract;

namespace RestWithAspNet10_Scaffold.Auth.Tools
{
    public class SecurePasswordHasher : IPasswordHasher
    {
        private readonly PasswordHasher<object> _hasher = new();

        public string Hash(string password)
        {
            return _hasher.HashPassword(new object(), password);
        }

        public bool Verify(string password, string hashedPassword)
        {
            var result = _hasher.VerifyHashedPassword(
                new object(),
                hashedPassword,
                password);

            return result == PasswordVerificationResult.Success;
        }
    }
}