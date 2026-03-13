//using RestWithAspNet10.IntegrationTests.Base;
//using RestWithAspNet10.IntegrationTests.Fixtures;
//using RestWithAspNet10.IntegrationTests.Tools;
//using RestWithAspNet10_Scaffold.DTOs.Common;
//using RestWithAspNet10_Scaffold.DTOs.V1.Person;
//using System.Net;
//using System.Net.Http.Json;

//namespace RestWithAspNet10.IntegrationTests.Person
//{
//    [Collection("IntegrationTests")]
//    public class PersonControllerJsonTests
//         : AuthenticatedIntegrationTest
//    {
//        public PersonControllerJsonTests(
//            SqlServerFixture sqlServerFixture,
//            TestDatabaseFixture db)
//            : base(sqlServerFixture, db)
//        {
//        }

//        [Fact(DisplayName = "01 - Retorna uma lista de Person")]
//        public async Task GetAllPersonsShouldReturnPersonList()
//        {
//            await SetupAsync();

//            var response = await _client.GetAsync("/api/v1/person");

//            response.EnsureSuccessStatusCode();

//            var paged = await response.Content
//                .ReadFromJsonAsync<PagedResponse<PersonResponseDTO>>();

//            Assert.NotNull(paged);
//            Assert.NotEmpty(paged!.Items);
//            Assert.Equal(3, paged.Items.Count);
//        }

//        [Fact(DisplayName = "02 - Retorna PersonResponseDTO")]
//        public async Task GetPersonByIdShouldReturnPerson()
//        {
//            await SetupAsync();


//            var response = await _client.GetAsync("/api/v1/person/1");

//            response.EnsureSuccessStatusCode();

//            var person = await response.Content
//                .ReadFromJsonAsync<PersonResponseDTO>();

//            Assert.NotNull(person);
//            Assert.Equal(1, person!.Id);
//            Assert.Equal("Ayrton", person.FirstName);
//            Assert.Equal("Senna", person.LastName);
//            Assert.Equal("São Paulo - Brasil", person.Address);
//            Assert.Equal("Male", person.Gender);
//        }

//        [Fact(DisplayName = "03 - Create Person")]
//        public async Task CreatePersonShouldReturnCreatedPerson()
//        {
//            await SetupAsync();

//            var request = new PersonCreateDTO
//            {
//                FirstName = "Jane",
//                LastName = "Smith",
//                Address = "456 Elm St",
//                Gender = "Female"
//            };

//            var response = await _client
//                .PostAsJsonAsync("/api/v1/person", request);

//            response.EnsureSuccessStatusCode();

//            var created = await response.Content
//                .ReadFromJsonAsync<PersonResponseDTO>();

//            Assert.NotNull(created);
//            Assert.Equal(request.FirstName, created!.FirstName);
//            Assert.Equal(request.LastName, created.LastName);
//            Assert.Equal(request.Address, created.Address);
//            Assert.Equal(request.Gender, created.Gender);
//        }

//        [Fact(DisplayName = "04 - Update Person")]
//        public async Task UpdatePersonShouldReturnUpdatedPerson()
//        {
//            await SetupAsync();


//            var updateDto = new PersonUpdateDTO
//            {
//                Id = 1,
//                FirstName = "Ayrton",
//                LastName = "Senna do Brasil",
//                Address = "São Paulo - Brasil",
//                Gender = "Male"
//            };

//            var response = await _client
//                .PutAsJsonAsync($"/api/v1/person/{updateDto.Id}", updateDto);

//            response.EnsureSuccessStatusCode();

//            var updated = await response.Content
//                .ReadFromJsonAsync<PersonResponseDTO>();

//            Assert.NotNull(updated);
//            Assert.Equal(updateDto.FirstName, updated!.FirstName);
//            Assert.Equal(updateDto.LastName, updated.LastName);
//            Assert.Equal(updateDto.Address, updated.Address);
//            Assert.Equal(updateDto.Gender, updated.Gender);
//        }

//        [Fact(DisplayName = "05 - Delete Person")]
//        public async Task DeletePersonShouldReturnNoContent()
//        {
//            await SetupAsync();

//            var response = await _client
//                .DeleteAsync("/api/v1/person/1");

//            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
//        }

//        [Fact(DisplayName = "06 - Enable Person")]
//        public async Task EnablePersonShouldReturnEnabledPerson()
//        {
//            await SetupAsync();

//            var response = await _client
//                .PatchAsync("/api/v1/person/2/enable", null);

//            response.EnsureSuccessStatusCode();

//            var enabled = await response.Content
//                .ReadFromJsonAsync<PersonResponseDTO>();

//            Assert.NotNull(enabled);
//            Assert.Equal(2, enabled!.Id);
//            Assert.True(enabled.Enabled);
//        }

//        [Fact(DisplayName = "07 - Disable Person")]
//        public async Task DisablePersonShouldReturnDisabledPerson()
//        {
//            await SetupAsync();

//            var response = await _client
//                .PatchAsync("/api/v1/person/2/disable", null);

//            response.EnsureSuccessStatusCode();

//            var disabled = await response.Content
//                .ReadFromJsonAsync<PersonResponseDTO>();

//            Assert.NotNull(disabled);
//            Assert.Equal(2, disabled!.Id);
//            Assert.False(disabled.Enabled);
//        }

//        [Fact(DisplayName = "08 - Get Person NotFound")]
//        public async Task GetPersonByIdShouldReturnNotFound()
//        {
//            await SetupAsync();

//            var response = await _client
//                .GetAsync("/api/v1/person/9999");

//            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
//        }

//        [Fact(DisplayName = "09 - Get Person by id")]
//        public async Task GetPersonByIdShouldReturnPersonResponseDTO()
//        {
//            await SetupAsync();

//            var response = await _client
//                .GetAsync("/api/v1/person/1");

//            response.EnsureSuccessStatusCode();

//            var person = await response.Content
//                .ReadFromJsonAsync<PersonResponseDTO>();

//            Assert.NotNull(person);
//            Assert.Equal(1, person!.Id);
//        }
//    }
//}