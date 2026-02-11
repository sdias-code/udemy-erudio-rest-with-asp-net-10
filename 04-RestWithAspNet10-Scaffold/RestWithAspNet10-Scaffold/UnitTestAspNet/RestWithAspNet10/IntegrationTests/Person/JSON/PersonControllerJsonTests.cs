using RestWithAspNet10.IntegrationTests.Tools;
using RestWithAspNet10_Scaffold.DTOs.V1.Person;
using System.Net.Http.Json;

namespace RestWithAspNet10.IntegrationTests.Person
{
    [TestCaseOrderer("RestWithAspNet10.IntegrationTests.Tools.PriorityOrderer", "RestWithAspNet10.IntegrationTests")]
    public class PersonControllerJsonTests : IClassFixture<SqlServerFixture>
    {
        private readonly HttpClient _client;        

        public PersonControllerJsonTests(SqlServerFixture sqlServerFixture)
        {

            var factory = new CustomWebApplicationFactory<Program>
                (sqlServerFixture.ConnectionString);

            _client = factory.CreateClient();
           
        }
    

        [Fact(DisplayName = "01 - Retorna uma lista de Person"), TestPriority(1)]
        public async Task GetAllPersonsShouldReturnPersonList()
        {

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/person");

            // Act
            var response = await _client.SendAsync(request);

            // Assert — status HTTP
            response.EnsureSuccessStatusCode();

            // Assert — desserializa para lista
            var persons = await response.Content
                .ReadFromJsonAsync<List<PersonResponseDTO>>();

            Assert.NotNull(persons);
            Assert.True(persons.Count >= 3);

        }

       
        [Fact(DisplayName = "02 - Retorna PersonResponseDTO"), TestPriority(2)]
        public async Task GetPersonByIdShouldReturnPerson()
        {
            // Arrange
         
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/person/1");


            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();

            var person = await response.Content.ReadFromJsonAsync<PersonResponseDTO>();
            
            Assert.NotNull(person);
            Assert.Equal(1, person!.Id);
            Assert.Equal("Ayrton", person.FirstName);
            Assert.Equal("Senna", person.LastName);
            Assert.Equal("São Paulo - Brasil", person.Address);
            Assert.Equal("Male", person.Gender);

        }
                  

        [Fact(DisplayName = "03 - Create Person"), TestPriority(3)]
        public async Task CreatePersonShouldReturnCreatedPerson()
        {
            // Arrange
           

            var request = new PersonCreateDTO
            {
                FirstName = "Jane",
                LastName = "Smith",
                Address = "456 Elm St",
                Gender = "Female"                
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1/person", request);

            // Assert
            response.EnsureSuccessStatusCode();

            var created = await response.Content.ReadFromJsonAsync<PersonCreateDTO>();

            Assert.NotNull(created);
            Assert.Equal(request.FirstName, created!.FirstName);
            Assert.Equal(request.LastName, created.LastName);
            Assert.Equal(request.Address, created.Address);
            Assert.Equal(request.Gender, created.Gender);

        }


        [Fact(DisplayName = "04 - Update Person"), TestPriority(4)]
        public async Task UpdatePersonShouldReturnUpdatePerson()
        {
            // Arrange
            var _personUpdateDTO = new PersonUpdateDTO
            {
                Id = 1,
                FirstName = "Ayrton",
                LastName = "Senna do Brasil",
                Address = "São Paulo - Brasil",
                Gender = "Male"
            };

           

            // Act
            var response = await _client.PutAsJsonAsync($"/api/v1/person/{_personUpdateDTO.Id}", _personUpdateDTO);

            // Assert
            response.EnsureSuccessStatusCode();

            var updated  = await response.Content.ReadFromJsonAsync<PersonResponseDTO>();

            Assert.NotNull(updated);
            Assert.Equal(updated.FirstName, _personUpdateDTO!.FirstName);
            Assert.Equal(updated.LastName, _personUpdateDTO.LastName);
            Assert.Equal(updated.Address, _personUpdateDTO.Address);
            Assert.Equal(updated.Gender, _personUpdateDTO?.Gender);

        }

    }
}
