using System.ComponentModel.DataAnnotations;

namespace RestWithAspNet10_Scaffold.DTOs.Book
{
    public class BookCreateDTO
    {
        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Author { get; set; } = null!;

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public DateTime LaunchDate { get; set; }
    }
}
