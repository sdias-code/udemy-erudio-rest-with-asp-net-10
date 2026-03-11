using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using RestWithAspNet10_Scaffold.DTOs.V1.Person;

namespace RestWithAspNet10_Scaffold.Files.Exporters.Contract.Factory
{
    public class XlsxExporter : IFileExporter
    {
        public FileContentResult ExportFile(List<PersonResponseDTO> data, string fileName)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("People");

            // Add headers
            worksheet.Cell(1, 1).Value = "Id";
            worksheet.Cell(1, 2).Value = "First Name";
            worksheet.Cell(1, 3).Value = "Last Name";
            worksheet.Cell(1, 4).Value = "Address";
            worksheet.Cell(1, 5).Value = "Gender";
            worksheet.Cell(1, 6).Value = "Enabled";

            var headerRange = worksheet.Range(1, 1, 1, 6);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Add data
            int row = 2;
            foreach (var person in data)
            {
                worksheet.Cell(row, 1).Value = person.Id;
                worksheet.Cell(row, 2).Value = person.FirstName;
                worksheet.Cell(row, 3).Value = person.LastName;
                worksheet.Cell(row, 4).Value = person.Address;
                worksheet.Cell(row, 5).Value = person.Gender;
                worksheet.Cell(row, 6).Value = person.Enabled == true ? "Yes" : "No";

                row++;

            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            var fileBytes = stream.ToArray();

            var baseName = string.IsNullOrWhiteSpace(fileName)
                ? "people"
                : Path.GetFileNameWithoutExtension(Path.GetFileName(fileName));

            var safeName = $"{baseName}_{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx";

            return new FileContentResult(fileBytes, MediaTypes.ApplicationXlsx)
            {
                FileDownloadName = safeName
            };            

        }

    }
}