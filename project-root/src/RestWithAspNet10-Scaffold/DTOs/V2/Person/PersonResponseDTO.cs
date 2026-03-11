using RestWithAspNet10_Scaffold.Utils;
using System.Text.Json.Serialization;

namespace RestWithAspNet10_Scaffold.DTOs.V2.Person
{
    public class PersonResponseDTO
    {
        public long Id { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; } = null!;

        [JsonPropertyName("last_name")]
        public string? LastName { get; set; }

        public string? Address { get; set; }


        [JsonConverter(typeof(GenderSerializer))]
        public string? Gender { get; set; }     
        
        public bool Enabled { get; set; }

    }
}
