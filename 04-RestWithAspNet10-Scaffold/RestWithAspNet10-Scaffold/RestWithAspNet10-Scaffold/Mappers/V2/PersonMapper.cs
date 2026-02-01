using RestWithAspNet10_Scaffold.DTOs.V2.Person;
using RestWithAspNet10_Scaffold.Model;

namespace RestWithAspNet10_Scaffold.Mappers.V2
{
    public static class PersonMapper
    {
        public static Person ToEntity(this PersonCreateDTO dto)
        {
            return new Person
            {
                FirstName = dto.FirstName,

                LastName = dto.LastName
                    ?? throw new ArgumentException("LastName é obrigatório"),

                Address = dto.Address
                    ?? throw new ArgumentException("Address é obrigatório"),

                Gender = dto.Gender
                    ?? throw new ArgumentException("Gender é obrigatório")
            };
        }

        public static Person ToEntity(this PersonUpdateDTO dto, Person entity)
        {
            entity.FirstName = dto.FirstName ?? entity.FirstName;
            entity.LastName = dto.LastName ?? entity.LastName;
            entity.Address = dto.Address ?? entity.Address;
            entity.Gender = dto.Gender ?? entity.Gender;

            return entity;
        }



        public static PersonResponseDTO ToDTO(this Person p)
        {
            return new PersonResponseDTO
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Address = p.Address,
                Gender = p.Gender
            };
        }
    }
}
