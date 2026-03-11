using RestWithAspNet10_Scaffold.DTOs.V1.Email;

namespace RestWithAspNet10_Scaffold.Services
{
    public interface IEmailService
    {
        void SendSimpleEmail(EmailRequestDTO emailRequest);
        Task SendEmailWithAttachment(
            EmailRequestDTO emailRequest,
            IFormFile attachment);
    }
}
