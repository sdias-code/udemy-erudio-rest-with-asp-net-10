namespace RestWithAspNet10_Scaffold.DTOs.Common
{
    public class PagedResponse<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }

        public int TotalPages =>
            (int)Math.Ceiling((double)TotalItems / PageSize);

        public List<T> Items { get; set; } = new();

        // HATEOAS
        public List<LinkDTO> Links { get; set; } = new();
    }
}
