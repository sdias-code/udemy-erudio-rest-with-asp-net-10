using System.ComponentModel.DataAnnotations;

namespace RestWithAspNet10_Scaffold.DTOs.V1
{
    public class MultipleFilesUploadDTO
    {
        [Required]
        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
    }
}
