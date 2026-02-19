using Microsoft.EntityFrameworkCore;
using RestWithAspNet10_Scaffold.Data;
using RestWithAspNet10_Scaffold.DTOs.Common;
using RestWithAspNet10_Scaffold.Infrastructure.Query;
using RestWithAspNet10_Scaffold.Model;
using System.Linq.Expressions;

namespace RestWithAspNet10_Scaffold.Repositories.Implementation
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Book?> FindByIdAsync(long id)
            => await _context.Books.AsNoTracking()
                                   .FirstOrDefaultAsync(b => b.Id == id);

        public async Task<PagedResponse<Book>> FindAllAsync(
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
            var query = _context.Books
            .AsNoTracking()
            .ApplyFilters(
                search,
                launchFrom,
                launchTo,
                minPrice,
                maxPrice,
                sortBy,
                direction,
                SortMap);

            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResponse<Book>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                Items = items
            };
        }

        public async Task<Book> CreateAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book> UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await _context.Books.FindAsync(id);

            if (entity is null) return;

            _context.Books.Remove(entity);
            await _context.SaveChangesAsync();
        }

                     
        private static readonly Dictionary<string, Expression<Func<Book, object?>>> SortMap =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["title"] = b => b.Title,
            ["author"] = b => b.Author,
            ["price"] = b => b.Price,
            ["launchdate"] = b => b.LaunchDate,
            ["id"] = b => b.Id
        };


    }
}
