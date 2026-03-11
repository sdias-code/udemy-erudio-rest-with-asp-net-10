using RestWithAspNet10_Scaffold.DTOs.V1;

namespace RestWithAspNet10_Scaffold.Services
{
    public interface IFileServices
    {
        byte[] GetFile(string fileName);
        Task<FileDetailDTO> SaveFileToDisk(IFormFile file);
        Task<List<FileDetailDTO>> SaveFilesToDisk(List<IFormFile> files);
    }
}
