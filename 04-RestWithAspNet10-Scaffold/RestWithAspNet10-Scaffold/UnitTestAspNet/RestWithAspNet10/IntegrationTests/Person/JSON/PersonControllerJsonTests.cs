using RestWithAspNet10.IntegrationTests.Tools;
using RestWithAspNet10_Scaffold.DTOs.V1.Person;
using System.Net;
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

            var updated = await response.Content.ReadFromJsonAsync<PersonResponseDTO>();

            Assert.NotNull(updated);
            Assert.Equal(updated.FirstName, _personUpdateDTO!.FirstName);
            Assert.Equal(updated.LastName, _personUpdateDTO.LastName);
            Assert.Equal(updated.Address, _personUpdateDTO.Address);
            Assert.Equal(updated.Gender, _personUpdateDTO?.Gender);

        }

        [Fact(DisplayName = "05 - Delete Person"), TestPriority(5)]
        public async Task DeletePersonShouldReturnNoContent()
        {
            // Arrange
            var personId = 1;

            // Act
            var response = await _client.DeleteAsync($"/api/v1/person/{personId}");

            response.EnsureSuccessStatusCode();

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        }

        [Fact(DisplayName = "06 - Enable Person"), TestPriority(6)]
        public async Task EnablePersonShouldReturnEnabledPerson()
        {
            // Arrange
            var personId = 2;

            // Act
            var response = await _client.PatchAsync($"/api/v1/person/{personId}/enable", null);
            response.EnsureSuccessStatusCode();

            // Assert
            var enabledPerson = await response.Content.ReadFromJsonAsync<PersonResponseDTO>();

            Assert.NotNull(enabledPerson);
            Assert.Equal(personId, enabledPerson!.Id);
            Assert.True(enabledPerson.Enabled);
        }

        [Fact(DisplayName = "07 - Disable Person"), TestPriority(7)]
        public async Task DisablePersonShouldReturnDisabledPerson()
        {
            // Arrange
            var personId = 2;

            // Act
            var response = await _client.PatchAsync($"/api/v1/person/{personId}/disable", null);

            response.EnsureSuccessStatusCode();

            // Assert
            var disabledPerson = await response.Content.ReadFromJsonAsync<PersonResponseDTO>();

            Assert.NotNull(disabledPerson);
            Assert.Equal(personId, disabledPerson!.Id);
            Assert.False(disabledPerson.Enabled);
        }

        [Fact(DisplayName = "08 - Get Person by id"), TestPriority(8)]
        public async Task GetPersonByIdShouldReturnNotFound()
        {
            // Arrange
            var nonExistentId = 9999;

            // Act
            var response = await _client.GetAsync($"/api/v1/person/{nonExistentId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact(DisplayName = "09 - Get Person by id"), TestPriority(8)]
        public async Task GetPersonByIdShouldReturnPersonResponseDTO()
        {
            // Arrange
            var personId = 1;

            // Act
            var response = await _client.GetAsync($"/api/v1/person/{personId}");
            var person = await response.Content.ReadFromJsonAsync<PersonResponseDTO>();

            response.EnsureSuccessStatusCode();

            // Assert
            Assert.Equal(personId, person!.Id);


        }
    }
}
