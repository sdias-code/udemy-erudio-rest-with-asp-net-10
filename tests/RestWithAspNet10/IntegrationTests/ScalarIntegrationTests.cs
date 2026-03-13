//using FluentAssertions;
//using Microsoft.AspNetCore.Mvc.Testing;
//using RestWithAspNet10.IntegrationTests.Tools;

//namespace RestWithAspNet10.IntegrationTests
//{
//    public class ScalarIntegrationTests : IClassFixture<SqlServerFixture>
//    {
//        private readonly HttpClient _client;

//        public ScalarIntegrationTests(SqlServerFixture sqlServerFixture)
//        {

//            var factory = new CustomWebApplicationFactory<Program>(sqlServerFixture.ConnectionString);

//            _client = factory.CreateClient(
//                new WebApplicationFactoryClientOptions
//                {
//                    BaseAddress = new Uri("http://localhost")
//                });
//        }

//        [Fact]
//        public async Task Scalar_ShouldReturnScalarUI()
//        {
//        // Arrange
//            var requestUri = "/scalar";

//            // Act
//            var response = await _client.GetAsync(requestUri);

//            // Assert
//            response.EnsureSuccessStatusCode();

//            var content = await response.Content.ReadAsStringAsync();

//            content.Should().NotBeNullOrEmpty();            
//            content.Should().Contain("<title>ASP.NET 10 RESTful API with Swagger, whit Docker and Kubernetes</title>");
//            content.Should().Contain("<script src=\"scalar.js\"></script>");
//        }

//    }

//}
