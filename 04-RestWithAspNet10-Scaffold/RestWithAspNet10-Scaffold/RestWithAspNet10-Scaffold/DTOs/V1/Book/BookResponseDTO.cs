using RestWithAspNet10_Scaffold.Hypermedia;
using RestWithAspNet10_Scaffold.Hypermedia.Abstract;

namespace RestWithAspNet10_Scaffold.DTOs.V1.Book
{
    public class BookResponseDTO : ISupportsHypermedia
    {
        public long Id { get; set; }
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public decimal Price { get; set; }
        public DateTime LaunchDate { get; set; }

        public List<HypermediaLink> Links { get; set; } = [];
    }
}
