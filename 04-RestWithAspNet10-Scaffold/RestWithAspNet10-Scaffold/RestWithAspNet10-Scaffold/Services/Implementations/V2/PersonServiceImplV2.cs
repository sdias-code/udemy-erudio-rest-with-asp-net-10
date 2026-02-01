using RestWithAspNet10_Scaffold.DTOs.V2.Person;
using RestWithAspNet10_Scaffold.Mappers;
using RestWithAspNet10_Scaffold.Mappers.V2;
using RestWithAspNet10_Scaffold.Repositories;

namespace RestWithAspNet10_Scaffold.Services.Implementations.V2
{
    public class PersonServiceImplV2: IPersonServiceV2
    {
        private readonly IPersonRepository _repo;

        public PersonServiceImplV2(IPersonRepository repo)
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

            if (entity == null)
                throw new Exception("Pessoa não encontrada");

            dto.ToEntity(entity);

            _repo.Update(entity);

            return entity.ToDTO();
        }


        public void Delete(long id) => _repo.Delete(id);
    }
}
