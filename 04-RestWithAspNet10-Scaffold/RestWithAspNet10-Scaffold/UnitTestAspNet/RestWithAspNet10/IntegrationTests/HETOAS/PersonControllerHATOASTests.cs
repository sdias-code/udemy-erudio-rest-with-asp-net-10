using RestWithAspNet10.IntegrationTests.Fixtures;
using RestWithAspNet10.IntegrationTests.Tools;
using RestWithAspNet10_Scaffold.DTOs.V1.Person;
using System.Text;
using System.Text.Json;

namespace RestWithAspNet10.IntegrationTests.HETOAS
{
    [Collection("IntegrationTests")]
    public class PersonControllerHATOASTests : IClassFixture<SqlServerFixture>
    {
        private readonly HttpClient _client;
        private readonly TestDatabaseFixture _db;

        public PersonControllerHATOASTests(SqlServerFixture sqlServerFixture, TestDatabaseFixture db)
        {
            var factory = new CustomWebApplicationFactory<Program>
                (sqlServerFixture.ConnectionString);

            _db = db;

            _db.InitializeAsync(sqlServerFixture.ConnectionString)
                .GetAwaiter().GetResult();

            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllPersons_ShouldReturnPersonsWithHateoasLinks()
        {
            await _db.ResetAsync();
            await _db.SeedAsync();

            var response = await _client.GetAsync("/api/v1/person");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            // Ensure 'persons' is non-null to satisfy Assert.NotEmpty which expects a non-null IEnumerable.
            var persons = JsonSerializer.Deserialize<List<PersonResponseDTO>>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? new List<PersonResponseDTO>();

            Assert.NotEmpty(persons);

            var first = persons[0];

            Assert.Equal("Ayrton", first.FirstName);
            Assert.NotEmpty(first.Links);

            Assert.Contains(first.Links, l => l.Rel == "collection");
            Assert.Contains(first.Links, l => l.Rel == "self");
            Assert.Contains(first.Links, l => l.Rel == "create");
            Assert.Contains(first.Links, l => l.Rel == "update");
            Assert.Contains(first.Links, l => l.Rel == "patch");
            Assert.Contains(first.Links, l => l.Rel == "delete");
        }

        [Fact]
        public async Task GetPersonById_ShouldReturnPersonWithHateoasLinks()
        {
            await _db.ResetAsync();
            await _db.SeedAsync();

            var response = await _client.GetAsync("/api/v1/person/1");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var person = JsonSerializer.Deserialize<PersonResponseDTO>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(person);
            Assert.Equal(1, person.Id);
            Assert.NotEmpty(person.Links);

            Assert.Contains(person.Links, l => l.Rel == "self");
            Assert.Contains(person.Links, l => l.Rel == "update");
            Assert.Contains(person.Links, l => l.Rel == "delete");
        }

        [Fact]
        public async Task CreatePerson_ShouldReturnPersonWithHateoasLinks()
        {
            await _db.ResetAsync();
            await _db.SeedAsync();

            var newPerson = new
            {
                firstName = "Lewis",
                lastName = "Hamilton",
                address = "UK",
                gender = "Male"
            };

            var content = new StringContent(
                JsonSerializer.Serialize(newPerson),
                Encoding.UTF8,
                "application/json");

            var response = await _client.PostAsync("/api/v1/person", content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var created = JsonSerializer.Deserialize<PersonResponseDTO>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(created);
            Assert.True(created.Id > 0);
            Assert.NotEmpty(created.Links);

            Assert.Contains(created.Links, l => l.Rel == "self");
            Assert.Contains(created.Links, l => l.Rel == "update");
            Assert.Contains(created.Links, l => l.Rel == "delete");
        }

        [Fact]
        public async Task UpdatePerson_ShouldReturnUpdatedPersonWithHateoasLinks()
        {
            await _db.ResetAsync();
            await _db.SeedAsync();

            var updatedPerson = new
            {
                id = 1,
                firstName = "Ayrton",
                lastName = "Senna",
                address = "Brasil",
                gender = "Male",
                enabled = true
            };

            var content = new StringContent(
                JsonSerializer.Serialize(updatedPerson),
                Encoding.UTF8,
                "application/json");

            var response = await _client.PutAsync("/api/v1/person/1", content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var person = JsonSerializer.Deserialize<PersonResponseDTO>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(person);
            Assert.Equal("Brasil", person.Address);
            Assert.NotEmpty(person.Links);

            Assert.Contains(person.Links, l => l.Rel == "self");
            Assert.Contains(person.Links, l => l.Rel == "delete");
        }

        [Fact]
        public async Task DeletePerson_ShouldRemovePerson()
        {
            await _db.ResetAsync();
            await _db.SeedAsync();

            var deleteResponse = await _client.DeleteAsync("/api/v1/person/1");
            deleteResponse.EnsureSuccessStatusCode();

            var getResponse = await _client.GetAsync("/api/v1/person/1");

            Assert.Equal(System.Net.HttpStatusCode.NotFound, getResponse.StatusCode);
        }
    }
}
