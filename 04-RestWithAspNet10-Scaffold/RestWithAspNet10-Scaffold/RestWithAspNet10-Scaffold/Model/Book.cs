using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestWithAspNet10_Scaffold.Model
{
    public class Book
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [Column("title", TypeName = "varchar(MAX)")]
        public string? Title { get; set; }

        [Required]
        [Column("author", TypeName = "varchar(MAX)")]
        public string? Author { get; set; }

        [Required]
        [Column("price")]
        public decimal Price { get; set; }

        [Required]
        [Column("launch_date")]
        public DateTime LaunchDate { get; set; }
    }
}
