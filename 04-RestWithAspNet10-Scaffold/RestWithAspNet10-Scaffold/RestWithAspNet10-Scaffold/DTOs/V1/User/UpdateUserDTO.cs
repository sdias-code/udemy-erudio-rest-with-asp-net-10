namespace RestWithAspNet10_Scaffold.DTOs.V1.User
{
    public class UpdateUserDTO
    {
        public long Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Password { get; set; }
    }
}
