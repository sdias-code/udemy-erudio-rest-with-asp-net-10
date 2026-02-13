using RestWithAspNet10.IntegrationTests.Fixtures;
using RestWithAspNet10.IntegrationTests.Tools;
using RestWithAspNet10_Scaffold.DTOs.V1.Person;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace RestWithAspNet10.IntegrationTests.Person
{
    [TestCaseOrderer("RestWithAspNet10.IntegrationTests.Tools.PriorityOrderer", "RestWithAspNet10.IntegrationTests")]
    [Collection("IntegrationTests")]
    public class PersonControllerXmlTests : IClassFixture<SqlServerFixture>
    {
        private readonly HttpClient _client;
        private readonly TestDatabaseFixture _db;

        public PersonControllerXmlTests(SqlServerFixture sqlServerFixture, TestDatabaseFixture db)
        {

            var factory = new CustomWebApplicationFactory<Program>
                (sqlServerFixture.ConnectionString);

            _db = db;

            _db.InitializeAsync(sqlServerFixture.ConnectionString)
                .GetAwaiter().GetResult();

            _client = factory.CreateClient();

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/xml"));

        }


        [Fact(DisplayName = "01 - Retorna uma lista de Person"), TestPriority(1)]
        public async Task GetAllPersonsShouldReturnPersonList()
        {
            // Arrange
            await _db.ResetAsync();
            await _db.SeedAsync();

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/person");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();

            // Assert — desserializa para lista
            var persons = await XmlHelper.ReadFromXmlAsync<List<PersonResponseDTO>>(response);            

            Assert.NotNull(persons);
            Assert.True(persons.Count >= 3);

        }


        [Fact(DisplayName = "02 - Retorna PersonResponseDTO"), TestPriority(2)]
        public async Task GetPersonByIdShouldReturnPerson()
        {
            // Arrange
            await _db.ResetAsync();
            await _db.SeedAsync();

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/person/1");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();

            var person = await XmlHelper.ReadFromXmlAsync<PersonResponseDTO>(response);

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

            await _db.ResetAsync();

            var request = new PersonCreateDTO
            {
                FirstName = "Jane",
                LastName = "Smith",
                Address = "456 Elm St",
                Gender = "Female"
            };

            // Act
            var xml = XmlHelper.SerializeToXml(request);

            using var content = new StringContent(xml, Encoding.UTF8, "application/xml");

            var response = await _client.PostAsync("/api/v1/person", content);

            // Assert
            response.EnsureSuccessStatusCode();

            var created = await XmlHelper.ReadFromXmlAsync<PersonResponseDTO>(response);            

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
            await _db.ResetAsync();
            await _db.SeedAsync();

            var _personUpdateDTO = new PersonUpdateDTO
            {
                Id = 1,
                FirstName = "Ayrton",
                LastName = "Senna do Brasil",
                Address = "São Paulo - Brasil",
                Gender = "Male"
            };


            // Act
            var xml = XmlHelper.SerializeToXml(_personUpdateDTO);

            using var content = new StringContent(xml, Encoding.UTF8, "application/xml");

            var response = await _client.PutAsync(
                $"/api/v1/person/{_personUpdateDTO.Id}", 
                content);

            // Assert
            response.EnsureSuccessStatusCode();

            var updated = await XmlHelper.ReadFromXmlAsync<PersonResponseDTO>(response);                

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
            await _db.ResetAsync();
            await _db.SeedAsync();

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
            await _db.ResetAsync();
            await _db.SeedAsync();

            var personId = 2;

            // Act
            var response = await _client.PatchAsync($"/api/v1/person/{personId}/enable", null);
            response.EnsureSuccessStatusCode();

            // Assert
            var enabledPerson = await XmlHelper.ReadFromXmlAsync<PersonResponseDTO>(response);            

            Assert.NotNull(enabledPerson);
            Assert.Equal(personId, enabledPerson!.Id);
            Assert.True(enabledPerson.Enabled);
        }

        [Fact(DisplayName = "07 - Disable Person"), TestPriority(7)]
        public async Task DisablePersonShouldReturnDisabledPerson()
        {
            // Arrange
            await _db.ResetAsync();
            await _db.SeedAsync();

            var personId = 2;

            // Act
            var response = await _client.PatchAsync($"/api/v1/person/{personId}/disable", null);

            response.EnsureSuccessStatusCode();

            // Assert
            var disabledPerson = await XmlHelper.ReadFromXmlAsync<PersonResponseDTO>(response);            

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
            await _db.ResetAsync();
            await _db.SeedAsync();

            var personId = 1;

            // Act
            var response = await _client.GetAsync($"/api/v1/person/{personId}");
            var person = await XmlHelper.ReadFromXmlAsync<PersonResponseDTO>(response);

            response.EnsureSuccessStatusCode();

            // Assert
            Assert.Equal(personId, person!.Id);


        }
    }
}
