namespace RestWithAspNet10_Scaffold.DTOs.V2.Book
{
    public class BookUpdateDTO
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public decimal? Price { get; set; }
        public DateTime? LaunchDate { get; set; }
    }
}
