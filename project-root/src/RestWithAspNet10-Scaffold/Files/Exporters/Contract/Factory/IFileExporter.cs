using Microsoft.AspNetCore.Mvc;
using RestWithAspNet10_Scaffold.DTOs.V1.Person;

namespace RestWithAspNet10_Scaffold.Files.Exporters.Contract.Factory
{
    public interface IFileExporter
    {
        FileContentResult ExportFile(List<PersonResponseDTO> data, string fileName);
    }
}