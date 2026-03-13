//using RestWithAspNet10.IntegrationTests.Fixtures;
//using RestWithAspNet10.IntegrationTests.Tools;
//using RestWithAspNet10_Scaffold.DTOs.Common;
//using RestWithAspNet10_Scaffold.DTOs.V1.Person;
//using RestWithAspNet10_Scaffold.DTOs.V1.User;
//using RestWithAspNet10_Scaffold.DTOs.V1.Token;
//using System.Net;
//using System.Net.Http.Headers;
//using System.Net.Http.Json;
//using System.Text;

//namespace RestWithAspNet10.IntegrationTests.Person
//{
//    [TestCaseOrderer("RestWithAspNet10.IntegrationTests.Tools.PriorityOrderer",
//        "RestWithAspNet10.IntegrationTests")]
//    [Collection("IntegrationTests")]
//    public class PersonControllerXmlTests : IClassFixture<SqlServerFixture>
//    {
//        private readonly HttpClient _client;
//        private readonly TestDatabaseFixture _db;

//        public PersonControllerXmlTests(
//            SqlServerFixture sqlServerFixture,
//            TestDatabaseFixture db)
//        {
//            var factory = new CustomWebApplicationFactory<Program>(
//                sqlServerFixture.ConnectionString);

//            _db = db;

//            _db.InitializeAsync(sqlServerFixture.ConnectionString)
//                .GetAwaiter()
//                .GetResult();

//            _client = factory.CreateClient();
//        }

//        // =====================================================
//        // TESTES
//        // =====================================================

//        [Fact(DisplayName = "01 - Retorna uma lista de Person"), TestPriority(1)]
//        public async Task GetAllPersonsShouldReturnPersonList()
//        {
//            await _db.ResetAsync();
//            await _db.SeedAllAsync();
//            await AuthenticateAsync();

//            var request = CreateXmlRequest(HttpMethod.Get, "/api/v1/person");

//            var response = await _client.SendAsync(request);

//            response.EnsureSuccessStatusCode();

//            var paged = await XmlHelper
//                .ReadFromXmlAsync<PagedResponse<PersonResponseDTO>>(response);

//            Assert.NotNull(paged);
//            Assert.NotEmpty(paged.Items);
//        }

//        [Fact(DisplayName = "02 - Retorna PersonResponseDTO"), TestPriority(2)]
//        public async Task GetPersonByIdShouldReturnPerson()
//        {
//            await _db.ResetAsync();
//            await _db.SeedAllAsync();
//            await AuthenticateAsync();

//            var request = CreateXmlRequest(HttpMethod.Get, "/api/v1/person/1");

//            var response = await _client.SendAsync(request);

//            response.EnsureSuccessStatusCode();

//            var person = await XmlHelper
//                .ReadFromXmlAsync<PersonResponseDTO>(response);

//            Assert.NotNull(person);
//            Assert.Equal(1, person!.Id);
//            Assert.Equal("Ayrton", person.FirstName);
//            Assert.Equal("Senna", person.LastName);
//        }

//        [Fact(DisplayName = "03 - Create Person"), TestPriority(3)]
//        public async Task CreatePersonShouldReturnCreatedPerson()
//        {
//            await _db.ResetAsync();
//            await _db.SeedAllAsync();
//            await AuthenticateAsync();

//            var dto = new PersonCreateDTO
//            {
//                FirstName = "Jane",
//                LastName = "Smith",
//                Address = "456 Elm St",
//                Gender = "Female"
//            };

//            var xml = XmlHelper.SerializeToXml(dto);
//            var request = CreateXmlRequest(HttpMethod.Post, "/api/v1/person");

//            request.Content = new StringContent(
//                xml, Encoding.UTF8, "application/xml");

//            var response = await _client.SendAsync(request);

//            response.EnsureSuccessStatusCode();

//            var created = await XmlHelper
//                .ReadFromXmlAsync<PersonResponseDTO>(response);

//            Assert.NotNull(created);
//            Assert.Equal(dto.FirstName, created!.FirstName);
//        }

//        [Fact(DisplayName = "04 - Delete Person"), TestPriority(4)]
//        public async Task DeletePersonShouldReturnNoContent()
//        {
//            await _db.ResetAsync();
//            await _db.SeedAllAsync();
//            await AuthenticateAsync();

//            var request = CreateXmlRequest(
//                HttpMethod.Delete, "/api/v1/person/1");

//            var response = await _client.SendAsync(request);

//            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
//        }

//        // =====================================================
//        // AUTH
//        // =====================================================

//        private async Task AuthenticateAsync()
//        {
//            _client.DefaultRequestHeaders.Authorization = null;

//            // Força JSON para login
//            _client.DefaultRequestHeaders.Accept.Clear();
//            _client.DefaultRequestHeaders.Accept.Add(
//                new MediaTypeWithQualityHeaderValue("application/json"));

//            var loginResponse = await _client.PostAsJsonAsync(
//                "/api/auth/signin",
//                new UserDTO
//                {
//                    Username = "testuser",
//                    Password = "123456"
//                });

//            loginResponse.EnsureSuccessStatusCode();

//            var token = await loginResponse.Content
//                .ReadFromJsonAsync<TokenDTO>();

//            _client.DefaultRequestHeaders.Authorization =
//                new AuthenticationHeaderValue(
//                    "Bearer",
//                    token!.AccessToken);
//        }

//        // =====================================================
//        // HELPER XML
//        // =====================================================

//        private HttpRequestMessage CreateXmlRequest(
//            HttpMethod method, string url)
//        {
//            var request = new HttpRequestMessage(method, url);

//            request.Headers.Accept.Clear();
//            request.Headers.Accept.Add(
//                new MediaTypeWithQualityHeaderValue("application/xml"));

//            return request;
//        }
//    }
//}