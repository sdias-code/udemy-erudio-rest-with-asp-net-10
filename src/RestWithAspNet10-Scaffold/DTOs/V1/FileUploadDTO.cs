using System.ComponentModel.DataAnnotations;

namespace RestWithAspNet10_Scaffold.DTOs.V1
{
    public class FileUploadDTO
    {
        [Required]
        public IFormFile File { get; set; } = null!;
    }
}
