using RestWithAspNet10_Scaffold.DTOs.V2.Person;

namespace RestWithAspNet10_Scaffold.Services
{
    public interface IPersonServiceV2
    {
        PersonResponseDTO? FindById(long id);
        List<PersonResponseDTO> FindAll();
        PersonResponseDTO Create(PersonCreateDTO dto);
        PersonResponseDTO Update(PersonUpdateDTO dto);
        void Delete(long id);
    }
}
