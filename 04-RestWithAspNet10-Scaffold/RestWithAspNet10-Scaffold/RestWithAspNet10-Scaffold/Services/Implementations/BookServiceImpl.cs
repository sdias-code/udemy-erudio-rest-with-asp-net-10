using RestWithAspNet10_Scaffold.DTOs.Common;
using RestWithAspNet10_Scaffold.DTOs.V1.Book;
using RestWithAspNet10_Scaffold.Mappers;
using RestWithAspNet10_Scaffold.Repositories;

namespace RestWithAspNet10_Scaffold.Services.Implementations
{
    public class BookServiceImpl : IBookService
    {
        private readonly IBookRepository _repo;

        public BookServiceImpl(IBookRepository repo)
        {
            _repo = repo;
        }

        public async Task<BookResponseDTO?> FindByIdAsync(long id)
        {
            var entity = await _repo.FindByIdAsync(id);

            return entity?.ToDTO();
        }

        public async Task<PagedResponse<BookResponseDTO>> FindAllAsync(
            int page,
            int pageSize,
            string sortBy,
            string direction,
            string? search,
            DateTime? launchFrom,
            DateTime? launchTo,
            decimal? minPrice,
            decimal? maxPrice)
            {
            var (books, totalItems) = await _repo.FindAllAsync(
                page,
                pageSize,
                sortBy,
                direction,
                search,
                launchFrom,
                launchTo,
                minPrice,
                maxPrice
            );

            return new PagedResponse<BookResponseDTO>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                Items = books.Select(b => b.ToDTO()).ToList()
            };
        }

        public async Task<BookResponseDTO> CreateAsync(BookCreateDTO dto)
        {
            var entity = dto.ToEntity();
            var created = await _repo.CreateAsync(entity);
            return created.ToDTO();
        }

        public async Task<BookResponseDTO> UpdateAsync(BookUpdateDTO dto)
        {
            var entity = await _repo.FindByIdAsync(dto.Id);
            if (entity == null)
                throw new Exception("Livro não encontrado");

            entity.Title = dto.Title;
            entity.Author = dto.Author;

            if (dto.Price.HasValue)
                entity.Price = dto.Price.Value;

            var updated = await _repo.UpdateAsync(entity);
            return updated.ToDTO();
        }

        public async Task DeleteAsync(long id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
