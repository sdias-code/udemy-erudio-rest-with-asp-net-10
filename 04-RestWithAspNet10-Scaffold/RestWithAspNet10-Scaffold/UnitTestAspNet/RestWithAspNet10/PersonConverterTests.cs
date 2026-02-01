using FluentAssertions;
using RestWithAspNet10_Scaffold.DTOs.V1.Person;
using RestWithAspNet10_Scaffold.Mappers.V1;
using RestWithAspNet10_Scaffold.Model;

namespace RestWithAspNet10
{
    public class PersonConverterTests
    {       


        [Fact]
        public void Parse_ShouldConvertPersonDTOToPerson()
        {
            // Arrange
            var dto = new PersonCreateDTO
            {
                FirstName = "John",
                LastName = "Doe",
                Address = "123 Main",
                Gender = "Male"
            };

            //Act
            var entity = dto.ToEntity();


            // Assert
            //Assert.Equal(personCreateDTO.FirstName, person.FirstName);
            entity.Should().NotBeNull();
            entity.FirstName.Should().Be(dto.FirstName);
            entity.LastName.Should().Be(dto.LastName);
            entity.Address.Should().Be(dto.Address);
            entity.Gender.Should().Be(dto.Gender);

            entity.Should().BeEquivalentTo(dto, options => options
                .ExcludingMissingMembers());
        }

        [Fact]
        public void Parse_ShouldConvertPersonToPersonDTO()
        {
            // Arrange
            var entity = new Person
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Address = "123 Main",
                Gender = "Male"
            };

            
            // Act
            var dto = entity.ToDTO();


            // Assert
            dto.Should().NotBeNull();
            dto.Id.Should().Be(entity.Id);
            dto.FirstName.Should().Be(entity.FirstName);
            dto.LastName.Should().Be(entity.LastName);

            // Garantia extra de contrato
            dto.Should().BeEquivalentTo(entity, options => options
                .Including(p => p.Id)
                .Including(p => p.FirstName)
                .Including(p => p.LastName));
        }
    }
}
