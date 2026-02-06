using System.Text.Json.Serialization;

namespace RestWithAspNet10_Scaffold.DTOs.V1.Person
{
    public class PersonResponseDTO
    {
        public long Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        
        public string? Address { get; set; }
        
        public string? Gender { get; set; }
    }
}
