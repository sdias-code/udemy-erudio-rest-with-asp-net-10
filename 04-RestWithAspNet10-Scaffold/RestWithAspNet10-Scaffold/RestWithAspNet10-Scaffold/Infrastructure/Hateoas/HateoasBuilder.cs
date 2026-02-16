using RestWithAspNet10_Scaffold.DTOs.Common;

namespace RestWithAspNet10_Scaffold.Infrastructure.Hateoas
{
    public class HateoasBuilder
    {
        public static List<LinkDTO> BuildPagedLinks(
            string baseUrl,
            int page,
            int pageSize,
            int totalItems)
        {
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var links = new List<LinkDTO>
            {
                new($"{baseUrl}?page={page}&pageSize={pageSize}", "self", "GET")
            };

            if (page > 1)
                links.Add(new($"{baseUrl}?page={page - 1}&pageSize={pageSize}", "prev", "GET"));

            if (page < totalPages)
                links.Add(new($"{baseUrl}?page={page + 1}&pageSize={pageSize}", "next", "GET"));

            return links;
        }
    }
}
