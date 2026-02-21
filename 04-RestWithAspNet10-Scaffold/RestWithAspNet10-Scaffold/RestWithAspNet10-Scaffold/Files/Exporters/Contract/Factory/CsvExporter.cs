using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using RestWithAspNet10_Scaffold.DTOs.V1.Person;
using System.Globalization;
using System.Text;

namespace RestWithAspNet10_Scaffold.Files.Exporters.Contract.Factory
{
    internal class CsvExporter : IFileExporter
    {
        public FileContentResult ExportFile(List<PersonResponseDTO> people)
        {
            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(
                memoryStream, 
                Encoding.UTF8,
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

            csvWriter.WriteRecords(people);

            streamWriter.Flush();

            var fileBytes = memoryStream.ToArray();

            return new FileContentResult(fileBytes, MediaTypes.ApplicationCsv)
            {
                FileDownloadName = $"people_exported_{DateTime.UtcNow:yyyyMMddHHmmss}.csv"
            };




        }
    }
}