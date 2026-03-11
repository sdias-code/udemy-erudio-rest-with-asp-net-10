using System.ComponentModel.DataAnnotations;

namespace RestWithAspNet10_Scaffold.DTOs.V1.User
{
    public class UserDTO
    {
        public UserDTO() { }

        public string Username { get; set; } = string.Empty;
        
        public string Password { get; set; } = string.Empty;
    }
}
