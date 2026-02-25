using RestWithAspNet10_Scaffold.Model.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestWithAspNet10_Scaffold.Model
{
    [Table("users")]
    public class User : BaseEntity
    {
        [Column("user_name")]
        public string Username { get; set; } = string.Empty;

        [Column("full_name")]
        public string FullName { get; set; } = string.Empty;

        [Column("password_hash")]
        public string PasswordHash { get; set; } = string.Empty;

        [Column("refresh_token")]
        public string? RefreshToken { get; set; }

        [Column("refresh_token_expiry_time")]
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
