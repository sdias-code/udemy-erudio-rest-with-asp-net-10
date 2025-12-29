namespace RestWithAspNet10_Scaffold.DTOs.Book
{
    public class BookResponseDTO
    {
        public long Id { get; set; }
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public decimal Price { get; set; }
        public DateTime LaunchDate { get; set; }
    }
}
