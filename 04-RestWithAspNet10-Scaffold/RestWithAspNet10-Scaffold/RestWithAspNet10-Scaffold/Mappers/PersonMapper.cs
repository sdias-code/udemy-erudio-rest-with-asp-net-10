using RestWithAspNet10_Scaffold.DTOs.Person;
using RestWithAspNet10_Scaffold.Model;

namespace RestWithAspNet10_Scaffold.Mappers
{
    public static class PersonMapper
    {
        public static Person ToEntity(this PersonCreateDTO dto)
        {    
            return new Person
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Address = dto.Address,
                Gender = dto.Gender
            };
        }

        public static PersonResponseDTO ToDTO(this Person p)
        {
            return new PersonResponseDTO
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName
            };
        }
    }
}
