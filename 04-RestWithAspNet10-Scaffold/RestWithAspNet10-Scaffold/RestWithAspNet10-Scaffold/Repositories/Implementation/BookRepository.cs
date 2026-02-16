using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RestWithAspNet10_Scaffold.Data;
using RestWithAspNet10_Scaffold.Model;

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
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<(List<Book> Books, int TotalItems)> FindAllAsync(
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
            var offset = (page - 1) * pageSize;

            var allowedSortColumns = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "id", "id" },
                { "title", "title" },
                { "author", "author" },
                { "price", "price" },
                { "launchDate", "launch_date" }
            };

            if (!allowedSortColumns.ContainsKey(sortBy))
                sortBy = "id";

            direction = direction.Equals("DESC", StringComparison.OrdinalIgnoreCase)
                ? "DESC"
                : "ASC";

            var whereParts = new List<string>();
            var parameters = new List<SqlParameter>();

            if (!string.IsNullOrWhiteSpace(search))
            {
                whereParts.Add("(title LIKE @search OR author LIKE @search)");
                parameters.Add(new SqlParameter("@search", $"%{search}%"));
            }

            if (launchFrom.HasValue)
            {
                whereParts.Add("launch_date >= @launchFrom");
                parameters.Add(new SqlParameter("@launchFrom", launchFrom.Value));
            }

            if (launchTo.HasValue)
            {
                whereParts.Add("launch_date <= @launchTo");
                parameters.Add(new SqlParameter("@launchTo", launchTo.Value));
            }

            if (minPrice.HasValue)
            {
                whereParts.Add("price >= @minPrice");
                parameters.Add(new SqlParameter("@minPrice", minPrice.Value));
            }

            if (maxPrice.HasValue)
            {
                whereParts.Add("price <= @maxPrice");
                parameters.Add(new SqlParameter("@maxPrice", maxPrice.Value));
            }

            var whereClause = whereParts.Any()
                ? "WHERE " + string.Join(" AND ", whereParts)
                : "";

            // 🔢 QUERY DE TOTAL
            var countSql = $"""
                SELECT id
                FROM dbo.books
                {whereClause}
            """;

            var totalItems = await _context.Books
            .FromSqlRaw(countSql, parameters.ToArray())
            .AsNoTracking()
            .CountAsync();

            // 📄 QUERY PAGINADA
            var dataSql = $"""
                SELECT id, title, author, price, launch_date
                FROM dbo.books
                {whereClause}
                ORDER BY {allowedSortColumns[sortBy]} {direction}
                OFFSET @offset ROWS
                FETCH NEXT @pageSize ROWS ONLY
            """;

            parameters.Add(new SqlParameter("@offset", offset));
            parameters.Add(new SqlParameter("@pageSize", pageSize));

            var books = await _context.Books
                .FromSqlRaw(dataSql, parameters.ToArray())
                .AsNoTracking()
                .ToListAsync();

            return (books, totalItems);
        }

        public async Task<Book> CreateAsync(Book book)
        {
            await _context.Books.AddAsync(book);
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
            var entity = await _context.Books
                .FirstOrDefaultAsync(b => b.Id == id);

            if (entity != null)
            {
                _context.Books.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

    }
}
