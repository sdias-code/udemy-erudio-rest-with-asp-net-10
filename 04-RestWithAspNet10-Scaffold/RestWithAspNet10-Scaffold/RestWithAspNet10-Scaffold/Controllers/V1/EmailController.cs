using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestWithAspNet10_Scaffold.DTOs.V1;
using RestWithAspNet10_Scaffold.DTOs.V1.Email;
using RestWithAspNet10_Scaffold.Services;
using System.Text.Json;

namespace RestWithAspNet10_Scaffold.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize("Bearer")]
    public class EmailController(
         IEmailService emailService,
        ILogger<EmailController> logger
        ) : ControllerBase
    {
        private readonly IEmailService _emailService = emailService;
        private readonly ILogger<EmailController> _logger = logger;

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult SendEmail(
            [FromBody] EmailRequestDTO emailRequest
        )
        {
            _logger.LogInformation("Sending email to {to}", emailRequest.To);

            _emailService.SendSimpleEmail(emailRequest);

            return Ok("Email sent successfully");
        }

        [HttpPost("with-attachment")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendEmailWithAttachment(
            [FromForm] string emailRequest,
            [FromForm] FileUploadDTO attachment
        )
        {
            if (string.IsNullOrWhiteSpace(emailRequest))
            {
                return BadRequest("Email request is required");
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            EmailRequestDTO? emailRequestDto = JsonSerializer
                .Deserialize<EmailRequestDTO>(emailRequest, options);

            if (emailRequestDto == null)
            {
                _logger.LogWarning("Invalid email request data");
                return BadRequest("Invalid email request data");
            }

            if (attachment == null ||
                attachment.File == null ||
                attachment.File.Length == 0)
            {
                _logger.LogWarning("Attachment is null or empty");
                return BadRequest("Attachment is null or empty");
            }

            _logger.LogInformation(
                "Sending email with attachment to {to}",
                emailRequestDto.To);

            await _emailService.SendEmailWithAttachment(
                emailRequestDto, attachment.File);

            return Ok("Email with attachment sent successfully");
        }
    }
}
