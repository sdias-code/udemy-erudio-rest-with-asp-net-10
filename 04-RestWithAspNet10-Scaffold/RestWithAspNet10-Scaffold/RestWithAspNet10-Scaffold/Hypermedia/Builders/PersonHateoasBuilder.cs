using RestWithAspNet10_Scaffold.DTOs.V1.Person;

namespace RestWithAspNet10_Scaffold.Hypermedia.Builders
{
    public static class PersonHateoasBuilder
    {
        public static void Build(PersonResponseDTO dto)
        {
            var id = dto.Id;

            dto.Links.Add(new HypermediaLink
            {
                Rel = "collection",
                Href = "/api/v1/person",
                Action = "GET"
            });

            dto.Links.Add(new HypermediaLink
            {
                Rel = "self",
                Href = $"/api/v1/person/{id}",
                Action = "GET"
            });

            dto.Links.Add(new HypermediaLink
            {
                Rel = "create",
                Href = "/api/v1/person",
                Action = "POST"
            });

            dto.Links.Add(new HypermediaLink
            {
                Rel = "update",
                Href = $"/api/v1/person/{id}",
                Action = "PUT"
            });

            dto.Links.Add(new HypermediaLink
            {
                Rel = "delete",
                Href = $"/api/v1/person/{id}",
                Action = "DELETE"
            });
        }
    }
}
