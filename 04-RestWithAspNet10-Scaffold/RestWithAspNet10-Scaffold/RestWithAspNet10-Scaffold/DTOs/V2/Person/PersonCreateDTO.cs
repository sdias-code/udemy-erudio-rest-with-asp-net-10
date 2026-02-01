using RestWithAspNet10_Scaffold.Utils;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RestWithAspNet10_Scaffold.DTOs.V2.Person
{
    public class PersonCreateDTO
    {
        [Required]
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; } = null!;

        [Required]
        [JsonPropertyName("last_name")]
        public string LastName { get; set; } = null!;

        [Required]
        public string Address { get; set; } = null!;

        [Required]
        [JsonConverter(typeof(GenderSerializer))]
        public string Gender { get; set; } = null!;
    }

}
