//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using RestWithAspNet10.IntegrationTests.Tools;
//using RestWithAspNet10_Scaffold.DTOs.V1.Person;
//using System.Net.Http.Headers;
//using System.Net.Http.Json;

//namespace RestWithAspNet10.IntegrationTests.Person.JSON
//{
//    [Collection("IntegrationTests")]
//    public class PersonCorsIntegrationTests
//        : IClassFixture<SqlServerFixture>
//    {
//        private readonly HttpClient _client;

//        public PersonCorsIntegrationTests(SqlServerFixture fixture)
//        {
//            var factory = new CustomWebApplicationFactory<Program>(
//                fixture.ConnectionString);

//            _client = factory.CreateClient();

//            var configuration = factory.Services
//                .GetRequiredService<IConfiguration>();

//            var token = JwtTestHelper.GenerateToken(configuration);

//            _client.DefaultRequestHeaders.Authorization =
//                new AuthenticationHeaderValue("Bearer", token);
//        }

//        private void AddOriginHeader(HttpRequestMessage request, string origin)
//        {
//            request.Headers.Add("Origin", origin);
//        }

//        [Fact]
//        public async Task GetAllPersons_ShouldAllowRequestFromAllowedOrigin()
//        {
//            var origin = "http://localhost:8080";

//            var request = new HttpRequestMessage(
//                HttpMethod.Get, "/api/v1/person");

//            AddOriginHeader(request, origin);

//            var response = await _client.SendAsync(request);

//            Assert.True(
//                response.Headers.Contains("Access-Control-Allow-Origin"));
//        }

//        [Fact]
//        public async Task Should_Block_Invalid_Origin()
//        {
//            var request = new HttpRequestMessage(
//                HttpMethod.Get, "/api/v1/person");

//            AddOriginHeader(request, "http://evil.com");

//            var response = await _client.SendAsync(request);

//            Assert.False(
//                response.Headers.Contains("Access-Control-Allow-Origin"));
//        }

//        [Fact]
//        public async Task CreatePerson_WithAllowedOrigin_ShouldReturnCreated()
//        {
//            var origin = "http://localhost:8080";

//            var request = new HttpRequestMessage(
//                HttpMethod.Post, "/api/v1/person");

//            AddOriginHeader(request, origin);

//            request.Content = JsonContent.Create(new PersonCreateDTO
//            {
//                FirstName = "Jane",
//                LastName = "Smith",
//                Address = "Street",
//                Gender = "Female"
//            });

//            var response = await _client.SendAsync(request);

//            response.EnsureSuccessStatusCode();

//            Assert.True(
//                response.Headers.Contains("Access-Control-Allow-Origin"));
//        }
//    }
//}