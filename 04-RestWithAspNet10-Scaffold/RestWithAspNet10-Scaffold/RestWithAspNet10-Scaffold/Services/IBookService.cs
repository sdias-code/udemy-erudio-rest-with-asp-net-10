using RestWithAspNet10_Scaffold.DTOs.Common;
using RestWithAspNet10_Scaffold.DTOs.V1.Book;

namespace RestWithAspNet10_Scaffold.Services
{
    public interface IBookService
    {
        Task<BookResponseDTO?> FindByIdAsync(long id);

        Task<PagedResponse<BookResponseDTO>> FindAllAsync(
            int page,
            int pageSize,
            string sortBy,
            string direction,
            string? search,
            DateTime? launchFrom,
            DateTime? launchTo,
            decimal? minPrice,
            decimal? maxPrice);

        Task<BookResponseDTO> CreateAsync(BookCreateDTO dto);

        Task<BookResponseDTO> UpdateAsync(BookUpdateDTO dto);

        Task DeleteAsync(long id);
    }
}
