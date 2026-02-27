using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestWithAspNet10_Scaffold.DTOs.V1;
using RestWithAspNet10_Scaffold.Services;

namespace RestWithAspNet10_Scaffold.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class FileController(
        IFileServices fileServices,
        ILogger<FileController> logger ) : ControllerBase
    {
        private IFileServices _fileServices = fileServices;
        private readonly ILogger<FileController> _logger = logger;

        // Download a file by name
        [HttpGet("downloadFile/{fileName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(byte[]))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Produces("application/octet-stream")]
        public IActionResult DownloadFile(string fileName)
        {
            var buffer = _fileServices.GetFile(fileName);

            if (buffer == null || buffer.Length == 0)
                return NoContent();

            var contentType = $"application/{Path.GetExtension(fileName).TrimStart('.')}";

            return File(buffer, contentType, fileName);
        }

        // Upload a single file
        [HttpPost("uploadFile")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileDetailDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Produces("application/json", "application/xml")]
        //public async Task<IActionResult> UploadFile(IFormFile file)
        public async Task<IActionResult> UploadFile([FromForm] FileUploadDTO input)
        {
            var fileDetail = await _fileServices.SaveFileToDisk(input.File);

            _logger.LogInformation("File {fileName} uploaded successfully.", fileDetail.DocumentName);

            return Ok(fileDetail);
        }

        // Upload multiple files
        [HttpPost("uploadMultipleFiles")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<FileDetailDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> UploadMultipleFiles(
            [FromForm] MultipleFilesUploadDTO input
        )
        {
            var details = await _fileServices
                .SaveFilesToDisk(input.Files);
            return Ok(details);
        }
    }
}
