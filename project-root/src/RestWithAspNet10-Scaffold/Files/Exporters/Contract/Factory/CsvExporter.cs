using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using RestWithAspNet10_Scaffold.DTOs.V1.Person;
using System.Globalization;
using System.Text;

namespace RestWithAspNet10_Scaffold.Files.Exporters.Contract.Factory
{
    public class CsvExporter : IFileExporter
    {
        public FileContentResult ExportFile(List<PersonResponseDTO> data, string fileName)
        {
            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(
                memoryStream,
                new UTF8Encoding(true),
                leaveOpen: true);

            using var csvWriter = new CsvHelper.CsvWriter(
                streamWriter, 
                new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    Delimiter = ",",
                    Quote = '"',
                    Escape = '"',
                    NewLine = Environment.NewLine
                });

            csvWriter.WriteRecords(data);

            streamWriter.Flush();

            var fileBytes = memoryStream.ToArray();

            var baseName = string.IsNullOrWhiteSpace(fileName)
                ? "people"
                : Path.GetFileNameWithoutExtension(Path.GetFileName(fileName));

            var safeName = $"{baseName}_{DateTime.UtcNow:yyyyMMddHHmmss}.csv";            

            return new FileContentResult(fileBytes, MediaTypes.ApplicationCsv)
            {
                FileDownloadName = safeName
            };

        }
    }
}