using DocumentFormat.OpenXml.Spreadsheet;
using RestWithAspNet10_Scaffold.Auth.Contract;
using RestWithAspNet10_Scaffold.DTOs.V1.Account;
using RestWithAspNet10_Scaffold.DTOs.V1.User;
using RestWithAspNet10_Scaffold.Model;
using RestWithAspNet10_Scaffold.Repositories;

namespace RestWithAspNet10_Scaffold.Services.Implementations
{
    public class UserAuthServiceImpl(
        IUserRepository repository,
        IPasswordHasher passwordHasher) : IUserAuthService
    {
        private readonly IUserRepository
            _repository = repository;

        private readonly IPasswordHasher
            _passwordHasher = passwordHasher;

        public User? FindByUsername(string username)
        {
            return _repository.FindByUsername(username);
        }

        // Obs. Recurso didático não recomendado para produção, apenas para exemplificar o processo de criação de usuário e hash de senha.
        // Em um cenário real, a criação de usuário e o hash de senha devem ser tratados com muito mais cuidado, incluindo validação de dados,
        // políticas de senha, e possivelmente o uso de bibliotecas especializadas para segurança.

        public User Create(AccountCredentialsDTO dto)
        {
            if (dto == null 
                || string.IsNullOrEmpty(dto.Username) 
                || string.IsNullOrEmpty(dto.Fullname)
                || string.IsNullOrEmpty(dto.Password)) 
                throw new ArgumentNullException(nameof(dto));

            var entity = new User
            {
                UserName = dto.Username,
                FullName = dto.Fullname,
                PasswordHash = _passwordHasher.Hash(dto.Password),
                RefreshToken = string.Empty,
                RefreshTokenExpiryTime = null
            };

            _repository.Create(entity);

            _repository.Save();

            return entity;
        }

        public bool RevokeToken(string username)
        {
            var user = _repository.FindByUsername(username);

            if (user == null) return false;

            user.RefreshToken = null;

            _repository.Update(user);
            _repository.Save();

            return true;
        }        

        public void UpdateRefreshToken(User user)
        {
            _repository.Update(user);
            _repository.Save();
        }

        public User UpdateProfile(UpdateUserDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var existingUser = _repository.FindById(dto.Id);

            if (existingUser == null)
                throw new KeyNotFoundException(
                    $"User with Id '{dto.Id}' not found.");

            // Atualiza apenas campos permitidos
            existingUser.FullName = dto.FullName;

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                existingUser.PasswordHash =
                    _passwordHasher.Hash(dto.Password);
            }

            _repository.Update(existingUser);
            _repository.Save();

            return existingUser;
        }
    }
}
