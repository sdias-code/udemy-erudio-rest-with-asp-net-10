using System.ComponentModel.DataAnnotations;

namespace RestWithAspNet10_Scaffold.DTOs.Person
{
    public class PersonCreateDTO
    {
        [Required]
        public string FirstName { get; set; } = null!;

        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
    }
}
