using CsvHelper.Configuration;
using RestWithAspNet10_Scaffold.DTOs.V1.Person;
using System.Globalization;

namespace RestWithAspNet10_Scaffold.Files.Importers.Contract.Factory
{
    internal class CsvImporter : IFileImporter
    {
        public async Task<List<PersonCreateDTO>> ImportFileAsync(Stream fileStream)
        {
            using var reader = new StreamReader(fileStream);

            using var csv = new CsvHelper.CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                IgnoreBlankLines = true,
                TrimOptions = TrimOptions.Trim,
            });

            var people = new List<PersonCreateDTO>();

            await foreach (var record in csv.GetRecordsAsync<dynamic>())
            {
                var person = new PersonCreateDTO
                {
                    FirstName = record.first_name,
                    LastName = record.last_name,
                    Address = record.address,
                    Gender = record.gender
                };
                people.Add(person);
            }
            return people;
        }
    }
}