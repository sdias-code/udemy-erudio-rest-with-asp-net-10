using RestWithAspNet10_Scaffold.DTOs.V1.Email;
using RestWithAspNet10_Scaffold.Mail;

namespace RestWithAspNet10_Scaffold.Services.Implementations.V1
{
    public class EmailService : IEmailService
    {
        private readonly EmailSender _emailSender;
        private readonly ILogger<EmailService> _logger;

        public EmailService(EmailSender emailSender, ILogger<EmailService> logger)
        {
            _emailSender = emailSender;
            _logger = logger;
        }
        public void SendSimpleEmail(
            EmailRequestDTO emailRequest)
        {
            _emailSender
                .To(emailRequest.To)
                .WithSubject(emailRequest.Subject)
                .WithMessage(emailRequest.Body)
                .Send();
        }
        public async Task SendEmailWithAttachment(EmailRequestDTO emailRequest, IFormFile attachment)
        {
            if (emailRequest == null || attachment.Length == 0)
            {
                _logger.LogError("Invalid email request or attachment.");
                throw new ArgumentException("Email request and attachment must be provided.");
            }

            string tempFilePath = Path.Combine(
                Path.GetTempPath(),
                attachment.FileName);

            try
            {
                await using (var stream = new FileStream(
                    tempFilePath,
                    FileMode.Create))
                {
                    await attachment.CopyToAsync(stream);                   
                }

                _emailSender.To(emailRequest.To)
                       .WithSubject(emailRequest.Subject)
                       .WithMessage(emailRequest.Body)
                       .Attach(tempFilePath)
                       .Send();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email with attachment.");
                throw;
            }
            finally
            {
                if (File.Exists(tempFilePath))
                {
                    try
                    {
                        File.Delete(tempFilePath);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to delete temporary file: {TempFilePath}", tempFilePath);
                    }
                }
            }

        }
    }
}
