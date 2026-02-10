using RestWithAspNet10_Scaffold.DTOs.V1.Person;
using RestWithAspNet10_Scaffold.Mappers.V1;
using RestWithAspNet10_Scaffold.Repositories;

namespace RestWithAspNet10_Scaffold.Services.Implementations.V1
{
    public class PersonServiceImpl: IPersonService
    {
        private readonly IPersonRepository _repo;

        public PersonServiceImpl(IPersonRepository repo)
        {
            _repo = repo;
        }

        public PersonResponseDTO? FindById(long id)
        {
            var entity = _repo.GetById(id);
            return entity?.ToDTO();
        }

        public List<PersonResponseDTO> FindAll()
        {
            return _repo.FindAll().Select(p => p.ToDTO()).ToList();
        }

        public PersonResponseDTO Create(PersonCreateDTO dto)
        {
            var entity = dto.ToEntity();
            return _repo.Create(entity).ToDTO();
        }

        public PersonResponseDTO Update(PersonUpdateDTO dto)
        {
            var entity = _repo.GetById(dto.Id);

            if (entity == null) throw new Exception("Pessoa não encontrada");

            if (dto.FirstName != null) entity.FirstName = dto.FirstName;
            if (dto.LastName != null) entity.LastName = dto.LastName;
            if (dto.Address != null) entity.Address = dto.Address;
            if (dto.Gender != null) entity.Gender = dto.Gender;           

            return _repo.Update(entity).ToDTO();
        }

        public void Delete(long id) => _repo.Delete(id);

        public PersonResponseDTO? Disable(long id)
        {
            
            var entity = _repo.Disable(id);
            return entity?.ToDTO();

        }

        public PersonResponseDTO? Enable(long id)
        {

            var entity = _repo.Enable(id);
            return entity?.ToDTO();

        }


    }
}
