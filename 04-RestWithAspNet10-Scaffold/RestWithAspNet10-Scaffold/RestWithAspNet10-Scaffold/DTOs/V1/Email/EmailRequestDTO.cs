namespace RestWithAspNet10_Scaffold.DTOs.V1.Email
{
    public class EmailRequestDTO
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}
