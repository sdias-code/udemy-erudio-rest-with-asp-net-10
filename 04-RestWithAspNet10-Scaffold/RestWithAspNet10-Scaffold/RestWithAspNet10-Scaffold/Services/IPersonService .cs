using RestWithAspNet10_Scaffold.DTOs.V1.Person;

namespace RestWithAspNet10_Scaffold.Services
{
    public interface IPersonService
    {
        PersonResponseDTO? FindById(long id);
        List<PersonResponseDTO> FindAll();
        PersonResponseDTO? Enable(long id);
        PersonResponseDTO? Disable(long id);
        PersonResponseDTO Create(PersonCreateDTO dto);
        PersonResponseDTO Update(PersonUpdateDTO dto);
        void Delete(long id);
    }
}
