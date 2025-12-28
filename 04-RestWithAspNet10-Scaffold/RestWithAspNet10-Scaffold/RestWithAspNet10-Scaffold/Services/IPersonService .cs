using RestWithAspNet10_Scaffold.DTOs.Person;

namespace RestWithAspNet10_Scaffold.Services
{
    public interface IPersonService
    {
        PersonResponseDTO? FindById(long id);
        List<PersonResponseDTO> FindAll();
        PersonResponseDTO Create(PersonCreateDTO dto);
        PersonResponseDTO Update(PersonUpdateDTO dto);
        void Delete(long id);
    }
}
