using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestWithAspNet10_Scaffold.Model
{
    [Table("person")]
    public class Person
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [Column("first_name", TypeName = "varchar(80)")]
        [MaxLength(80)]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last Name is required")]
        [Column("last_name", TypeName = "varchar(80)")]
        [MaxLength(80)]
        public string LastName { get; set; } = null!;

        [Required]
        [Column("address", TypeName = "varchar(100)")] // ✅ CORRIGIDO
        [MaxLength(100)]
        public string Address { get; set; } = null!;

        [Required]
        [Column("gender", TypeName = "varchar(6)")]
        [MaxLength(6)]
        public string Gender { get; set; } = null!;
    }
}
