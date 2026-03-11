using System.Linq.Expressions;

namespace RestWithAspNet10_Scaffold.Infrastructure.Query
{
    public static class SortingExtensions
    {
        public static IQueryable<T> ApplySorting<T>(
            this IQueryable<T> query,
            string sortBy,
            string direction,
            IReadOnlyDictionary<string, Expression<Func<T, object?>>> map)
        {
            if (!map.TryGetValue(sortBy, out var expression))
                return query;

            var asc = direction.Equals("asc", StringComparison.OrdinalIgnoreCase);

            return asc
                ? query.OrderBy(expression)
                : query.OrderByDescending(expression);
        }
    }
}
