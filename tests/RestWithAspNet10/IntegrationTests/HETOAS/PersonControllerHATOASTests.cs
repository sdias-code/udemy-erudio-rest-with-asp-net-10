//using RestWithAspNet10.IntegrationTests.Base;
//using RestWithAspNet10.IntegrationTests.Fixtures;
//using RestWithAspNet10.IntegrationTests.Tools;
//using RestWithAspNet10_Scaffold.DTOs.Common;
//using RestWithAspNet10_Scaffold.DTOs.V1.Person;
//using System.Net;
//using System.Text;
//using System.Text.Json;

//namespace RestWithAspNet10.IntegrationTests.HETOAS
//{
//    [Collection("IntegrationTests")]
//    public class PersonControllerHATOASTests : AuthenticatedIntegrationTest
//    {
//        public PersonControllerHATOASTests(
//            SqlServerFixture fixture, 
//            TestDatabaseFixture db) : base(fixture, db)
//        {
//        }

//        [Fact]
//        public async Task GetAllPerson_ShouldReturnPersonWithHateoasLinks()
//        {
//            await SetupAsync();

//            var response = await _client.GetAsync("/api/v1/person");
//            response.EnsureSuccessStatusCode();

//            var json = await response.Content.ReadAsStringAsync();

//            var responseData = JsonSerializer.Deserialize<PagedResponse<PersonResponseDTO>>(
//                json,
//                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

//            Assert.NotNull(responseData);
//            Assert.NotEmpty(responseData.Items);

//            var first = responseData.Items[0];

//            Assert.NotEmpty(first.Links);
//            Assert.Contains(first.Links, l => l.Rel == "collection");
//            Assert.Contains(first.Links, l => l.Rel == "self");
//            Assert.Contains(first.Links, l => l.Rel == "create");
//            Assert.Contains(first.Links, l => l.Rel == "update");
//            Assert.Contains(first.Links, l => l.Rel == "delete");
//        }


//        [Fact]
//        public async Task GetPersonById_ShouldReturnPersonWithHateoasLinks()
//        {
//            // Arrange
//            await SetupAsync();

//            // Act
//            var response = await _client.GetAsync("/api/v1/person/1");
//            response.EnsureSuccessStatusCode();

//            var json = await response.Content.ReadAsStringAsync();

//            var person = JsonSerializer.Deserialize<PersonResponseDTO>(
//                json,
//                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

//            // Assert
//            Assert.NotNull(person);
//            Assert.NotEmpty(person.Links);

//            Assert.Contains(person.Links, l => l.Rel == "self");
//            Assert.Contains(person.Links, l => l.Rel == "update");
//            Assert.Contains(person.Links, l => l.Rel == "delete");
//        }

//        [Fact]
//        public async Task CreatePerson_ShouldReturnPersonkWithHateoasLinks()
//        {
//            // Arrange
//            await SetupAsync();

//            var person = new
//            {
//                firstName = "John",
//                lastName = "Doe",
//                address = "123 Main",
//                gender = "Male"
//            };

//            var content = new StringContent(
//                JsonSerializer.Serialize(person),
//                Encoding.UTF8,
//                "application/json");

//            // Act
//            var response = await _client.PostAsync("/api/v1/person", content);
//            response.EnsureSuccessStatusCode();

//            var json = await response.Content.ReadAsStringAsync();

//            var created = JsonSerializer.Deserialize<PersonResponseDTO>(
//                json,
//                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

//            // Assert
//            Assert.NotNull(created);
//            Assert.True(created.Id > 0);
//            Assert.NotEmpty(created.Links);

//            Assert.Contains(created.Links, l => l.Rel == "self");
//            Assert.Contains(created.Links, l => l.Rel == "update");
//            Assert.Contains(created.Links, l => l.Rel == "delete");
//        }

//        [Fact]
//        public async Task UpdatePerson_ShouldReturnUpdatedPersonWithHateoasLinks()
//        {
//            // Arrange
//            await SetupAsync();

//            var updatedPerson = new
//            {
//                id = 1,
//                firstName = "Updated First Name",
//                lastName = "Updated Last Name",
//                address = "Updated Address"
//            };

//            var content = new StringContent(
//                JsonSerializer.Serialize(updatedPerson),
//                Encoding.UTF8,
//                "application/json");

//            // Act
//            var response = await _client.PutAsync("/api/v1/person/1", content);
//            response.EnsureSuccessStatusCode();

//            var json = await response.Content.ReadAsStringAsync();

//            var person = JsonSerializer.Deserialize<PersonResponseDTO>(
//                json,
//                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

//            // Assert
//            Assert.NotNull(person);
//            Assert.NotEmpty(person.Links);
//            Assert.Contains(person.Links, l => l.Rel == "self");
//            Assert.Contains(person.Links, l => l.Rel == "delete");
//        }

//        [Fact]
//        public async Task DeletePerson_ShouldRemovePerson()
//        {
//            // Arrange
//            await SetupAsync();

//            // Act
//            var deleteResponse = await _client.DeleteAsync("/api/v1/person/1");

//            // Assert
//            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

//            var getResponse = await _client.GetAsync("/api/v1/person/1");
//            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
//        }
//    }
//}
