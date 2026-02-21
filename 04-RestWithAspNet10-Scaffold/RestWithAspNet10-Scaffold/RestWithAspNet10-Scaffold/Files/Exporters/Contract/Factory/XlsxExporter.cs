using Microsoft.AspNetCore.Mvc;
using RestWithAspNet10_Scaffold.DTOs.V1.Person;

namespace RestWithAspNet10_Scaffold.Files.Exporters.Contract.Factory
{
    internal class XlsxExporter : IFileExporter
    {
        public FileContentResult ExportFile(List<PersonResponseDTO> people)
        {
            throw new NotImplementedException();
        }
    }
}