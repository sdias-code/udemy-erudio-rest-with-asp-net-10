using ClosedXML.Excel;
using RestWithAspNet10_Scaffold.DTOs.V1.Person;

namespace RestWithAspNet10_Scaffold.Files.Importers.Contract.Factory
{
    internal class XlsxImporter : IFileImporter
    {       

        public Task<List<PersonCreateDTO>> ImportFileAsync(Stream fileStream)
        {
            var people = new List<PersonCreateDTO>();

            var workbook = new XLWorkbook(fileStream); // Load the workbook from the stream

            var worksheet = workbook.Worksheets.First(); // Get the first worksheet

            var rows = worksheet.RowsUsed().Skip(1); // Skip header row

            foreach (var row in rows)
            {
                if (!row.Cell(1).IsEmpty())
                {
                    people.Add(
                        new PersonCreateDTO
                        {
                            FirstName = row.Cell(1).GetValue<string>(),
                            LastName = row.Cell(2).GetValue<string>(),
                            Address = row.Cell(3).GetValue<string>(),
                            Gender = row.Cell(4).GetValue<string>()                           
                        });
                }

            }

            return Task.FromResult(people);
        }
    }
}