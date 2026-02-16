using RestWithAspNet10_Scaffold.Model;

namespace RestWithAspNet10_Scaffold.Repositories
{
    public interface IBookRepository
    {
        Task<Book?> FindByIdAsync(long id);

        Task<(List<Book> Books, int TotalItems)> FindAllAsync(
            int page,
            int pageSize,
            string sortBy,
            string direction,
            string? search,
            DateTime? launchFrom,
            DateTime? launchTo,
            decimal? minPrice,
            decimal? maxPrice);

        Task<Book> CreateAsync(Book book);

        Task<Book> UpdateAsync(Book book);

        Task DeleteAsync(long id);
    }
}
