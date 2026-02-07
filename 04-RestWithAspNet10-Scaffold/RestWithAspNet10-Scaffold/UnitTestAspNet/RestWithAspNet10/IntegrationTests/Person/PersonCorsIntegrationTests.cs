using Microsoft.AspNetCore.Mvc.Testing;
using RestWithAspNet10.IntegrationTests.Tools;
using RestWithAspNet10_Scaffold.DTOs.V1.Person;
using System.Net;
using System.Net.Http.Json;

namespace RestWithAspNet10.IntegrationTests.Person
{
    [TestCaseOrderer("RestWithAspNet10.IntegrationTests.Tools.PriorityOrderer", "RestWithAspNet10.IntegrationTests")]
    public class PersonCorsIntegrationTests : IClassFixture<SqlServerFixture>
    {
        private readonly HttpClient _client;

        public PersonCorsIntegrationTests(SqlServerFixture sqlServerFixture)
        {

            var factory = new CustomWebApplicationFactory<Program>
                (sqlServerFixture.ConnectionString);

            _client = factory.CreateClient();
        }

        private void AddOriginHeader(HttpRequestMessage request, string origin)
        {
            request.Headers.Add("Origin", origin);
        }

        [Fact, TestPriority(1)]
        public async Task Should_Block_Invalid_Origin()
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get, "/api/v1/person");

            request.Headers.Add("Origin", "http://evil.com");

            var response = await _client.SendAsync(request);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact, TestPriority(2)]
        public async Task GetAllPersons_ShouldAllowRequestFromAllowedOrigin()
        {
            // Arrange
            var allowedOrigins = new[]
            {
                "http://localhost:8080",
                "http://localhost:3000"
            };

            var origin = allowedOrigins.First();

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/person");

            AddOriginHeader(request, origin);

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.True(
                response.Headers.Contains("Access-Control-Allow-Origin"));
            var corsHeader = response.Headers
                .GetValues("Access-Control-Allow-Origin")
                .First();
            Assert.Equal(origin, corsHeader);

        }


        [Fact, TestPriority(3)]
        public async Task GetAllPersons_ShouldBlockRequestFromNotAllowedOrigin()
        {
            // Arrange
            var notAllowedOrigin = "http://malicious-website.com";
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/person");
            AddOriginHeader(request, notAllowedOrigin);

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.False(
                response.Headers.Contains("Access-Control-Allow-Origin"));
        }


        [Fact, TestPriority(4)]
        public async Task GetPersonById_ShouldAllowRequestFromAllowedOrigin()
        {
            // Arrange
            var allowedOrigins = new[]
            {
                "http://localhost:8080",
                "http://localhost:3000"
            };

            var origin = allowedOrigins.First();
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/person/1");

            AddOriginHeader(request, origin);

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.True(
                response.Headers.Contains("Access-Control-Allow-Origin"));
            var corsHeader = response.Headers
                .GetValues("Access-Control-Allow-Origin")
                .First();
            Assert.Equal(origin, corsHeader);
        }


        [Fact, TestPriority(5)]
        public async Task GetPersonById_ShouldBlockRequestFromNotAllowedOrigin()
        {
            // Arrange
            var notAllowedOrigin = "http://malicious-website.com";
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/person/1");

            AddOriginHeader(request, notAllowedOrigin);

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.False(
                response.Headers.Contains("Access-Control-Allow-Origin"));
        }


        [Fact, TestPriority(6)]
        public async Task CreatePerson_ShouldAllowRequestFromAllowedOrigin()
        {
            // Arrange
            var allowedOrigins = new[]
            {
                "http://localhost:8080",
                "http://localhost:3000"
            };

            var origin = allowedOrigins.First();
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/person");

            AddOriginHeader(request, origin);

            request.Content = new StringContent(
                "{\"firstName\":\"John\",\"lastName\":\"Doe\",\"address\":\"123 Main St\",\"gender\":\"Male\"}",
                System.Text.Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.True(
                response.Headers.Contains("Access-Control-Allow-Origin"));

            var corsHeader = response.Headers
                .GetValues("Access-Control-Allow-Origin")
                .First();

            Assert.Equal(origin, corsHeader);
        }


        [Fact, TestPriority(7)]
        public async Task CreatePerson_ShouldBlockRequestFromNotAllowedOrigin()
        {
            // Arrange
            var notAllowedOrigin = "http://malicious-website.com";
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/person");

            AddOriginHeader(request, notAllowedOrigin);

            request.Content = new StringContent(
                "{\"firstName\":\"John\",\"lastName\":\"Doe\",\"address\":\"123 Main St\",\"gender\":\"Male\"}",
                System.Text.Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.False(
                response.Headers.Contains("Access-Control-Allow-Origin"));
        }


        [Fact, TestPriority(8)]
        public async Task OptionsRequest_ShouldReturnCorsHeadersForPreflightRequest()
        {
            // Arrange
            var allowedOrigins = new[]
            {
                "http://localhost:8080",
                "http://localhost:3000"
            };

            var origin = allowedOrigins.First();
            var request = new HttpRequestMessage(HttpMethod.Options, "/api/v1/person");

            AddOriginHeader(request, origin);
            request.Headers.Add("Access-Control-Request-Method", "POST");
            request.Headers.Add("Access-Control-Request-Headers", "Content-Type");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.True(
                response.Headers.Contains("Access-Control-Allow-Origin"));
            var corsHeader = response.Headers
                .GetValues("Access-Control-Allow-Origin")
                .First();
            Assert.Equal(origin, corsHeader);
            Assert.True(
                response.Headers.Contains("Access-Control-Allow-Methods"));
            Assert.True(
                response.Headers.Contains("Access-Control-Allow-Headers"));
        }


        [Fact, TestPriority(8)]
        public async Task CreatePerson_WithAllowedOrgin_SholdReturnCreated()
        {
            // Arrange
            var allowedOrigins = new[]
            {
                "http://localhost:8080",
                "http://localhost:3000"
            };

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

        }


    }
}
