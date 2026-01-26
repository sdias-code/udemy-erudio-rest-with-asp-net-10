using System.ComponentModel.DataAnnotations;

namespace RestWithAspNet10_Scaffold.DTOs.V2.Person
{
    public class PersonCreateDTO
    {
        [Required]
        public string FirstName { get; set; } = null!;

        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
