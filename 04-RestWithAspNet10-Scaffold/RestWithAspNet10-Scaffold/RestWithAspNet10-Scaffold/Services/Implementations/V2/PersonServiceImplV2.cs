using RestWithAspNet10_Scaffold.DTOs.Common;
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

        public async Task<PersonResponseDTO?> FindById(long id)
        {
            var entity = await _repo.GetById(id);

            return entity?.ToDTO();
        }

        public async Task<PagedResponse<PersonResponseDTO>> FindAll(
          int page,
          int pageSize,
          string sortBy,
          string direction,
          string? search)
        {
            var pagedPersons = await _repo.FindAll(
                page,
                pageSize,
                sortBy,
                direction,
                search);

            return new PagedResponse<PersonResponseDTO>
            {
                Page = pagedPersons.Page,
                PageSize = pagedPersons.PageSize,
                TotalItems = pagedPersons.TotalItems,
                Items = pagedPersons.Items
                    .Select(p => p.ToDTO())
                    .ToList()
            };
        }

        public async Task<PersonResponseDTO> Create(PersonCreateDTO dto)
        {
            var entity = dto.ToEntity();

            var created = await _repo.Create(entity);

            return created.ToDTO();
        }

        public async Task<PersonResponseDTO> Update(PersonUpdateDTO dto)
        {
            var entity = await _repo.GetById(dto.Id);

            if (entity == null)
                throw new Exception("Pessoa não encontrada");

            if (dto.FirstName != null) entity.FirstName = dto.FirstName;
            if (dto.LastName != null) entity.LastName = dto.LastName;
            if (dto.Address != null) entity.Address = dto.Address;
            if (dto.Gender != null) entity.Gender = dto.Gender;

            var updated = await _repo.Update(entity);

            return updated.ToDTO();
        }


        public async Task Delete(long id)
        {
            await _repo.Delete(id);
        }

        public async Task<PersonResponseDTO?> Enable(long id)
        {
            var entity = await _repo.Enable(id);
            return entity?.ToDTO();
        }
        public async Task<PersonResponseDTO?> Disable(long id)
        {
            var entity = await _repo.Disable(id);
            return entity?.ToDTO();
        }
    }
}
