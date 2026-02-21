using RestWithAspNet10_Scaffold.DTOs.Common;
using RestWithAspNet10_Scaffold.DTOs.V1.Person;
using RestWithAspNet10_Scaffold.Files.Importers.Contract.Factory;
using RestWithAspNet10_Scaffold.Mappers.V1;
using RestWithAspNet10_Scaffold.Model;
using RestWithAspNet10_Scaffold.Repositories;

namespace RestWithAspNet10_Scaffold.Services.Implementations.V1
{
    public class PersonServiceImpl : IPersonService
    {
        private readonly IPersonRepository _repo;
        private readonly FileImporterFactory _fileImporterFactory;
        private readonly ILogger<PersonServiceImpl> _logger;

        public PersonServiceImpl(
            IPersonRepository repo,
            FileImporterFactory fileImporterFactory,
            ILogger<PersonServiceImpl> logger)
        {
            _repo = repo;
            _fileImporterFactory = fileImporterFactory;
            _logger = logger;
        }

        public async Task<PagedResponse<PersonResponseDTO>> FindAllAsync(
            int page,
            int pageSize,
            string sortBy,
            string direction,
            string? search)
        {
            var pagedPersons = await _repo.FindAllAsync(
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

        public async Task<PersonResponseDTO?> FindByIdAsync(long id)
        {
            var entity = await _repo.GetByIdAsync(id);

            return entity?.ToDTO();
        }

        public async Task<PersonResponseDTO> CreateAsync(PersonCreateDTO dto)
        {
            var entity = dto.ToEntity();

            var created = await _repo.CreateAsync(entity);

            if (created == null)
                throw new Exception("Erro ao criar pessoa");

            return created.ToDTO();
        }

        public async Task<PersonResponseDTO?> UpdateAsync(long id, PersonUpdateDTO dto)
        {
            var entity = await _repo.GetByIdAsync(id);

            if (entity == null)
                return null;

            if (!string.IsNullOrWhiteSpace(dto.FirstName))
                entity.FirstName = dto.FirstName;

            if (!string.IsNullOrWhiteSpace(dto.LastName))
                entity.LastName = dto.LastName;

            if (!string.IsNullOrWhiteSpace(dto.Address))
                entity.Address = dto.Address;

            if (!string.IsNullOrWhiteSpace(dto.Gender))
                entity.Gender = dto.Gender;

            var updated = await _repo.UpdateAsync(entity);

            return updated.ToDTO();
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _repo.GetByIdAsync(id);

            if (entity == null)
                return false;

            await _repo.DeleteAsync(id);

            return true;
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

        public async Task<List<PersonResponseDTO>> ImportFromFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("File is null or empty");
                throw new ArgumentException("File is null or empty");
            }

            using var stream = file.OpenReadStream();
            var fileName = file.FileName;

            try
            {
                var importer = _fileImporterFactory.GetImporter(fileName);

                var personsDto = await importer.ImportFileAsync(stream);

                var entities = personsDto
                    .Select(dto => dto.ToEntity())
                    .ToList();

                var savedEntities = await _repo.CreateRangeAsync(entities);

                return savedEntities
                    .Select(entity => entity.ToDTO())
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing file");
                throw;
            }
        }
    }
}
