using Microsoft.EntityFrameworkCore;
using RestWithAspNet10_Scaffold.Model;
using System.Linq.Expressions;

namespace RestWithAspNet10_Scaffold.Infrastructure.Query
{
    public static class BookQueryBuilder
    {
        public static IQueryable<Book> ApplyFilters(
            this IQueryable<Book> query,
            string? search,
            DateTime? launchFrom,
            DateTime? launchTo,
            decimal? minPrice,
            decimal? maxPrice,
            string sortBy,
            string direction,
            Dictionary<string, Expression<Func<Book, object?>>> sortMap)
        {
            // 🔎 Search (Title ou Author)
            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = $"%{search}%";

                query = query.Where(b =>
                    EF.Functions.Like(b.Title, term) ||
                    EF.Functions.Like(b.Author, term));
            }

            // 📅 Filtro por data de lançamento
            if (launchFrom.HasValue)
                query = query.Where(b => b.LaunchDate >= launchFrom.Value);

            if (launchTo.HasValue)
                query = query.Where(b => b.LaunchDate <= launchTo.Value);

            // 💰 Filtro por preço
            if (minPrice.HasValue)
                query = query.Where(b => b.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(b => b.Price <= maxPrice.Value);

            // 🔃 Ordenação (usa seu ApplySorting já existente)
            query = query.ApplySorting(sortBy, direction, sortMap);

            return query;
        }
    }
}
