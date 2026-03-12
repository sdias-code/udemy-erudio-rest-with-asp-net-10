using RestWithAspNet10_Scaffold.DTOs.V1.Person;

namespace RestWithAspNet10_Scaffold.Files.Importers.Contract
{
    public interface IFileImporter
    {
        Task<List<PersonCreateDTO>> ImportFileAsync(Stream fileStream);
    }
}
