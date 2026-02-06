using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using RestWithAspNet10.IntegrationTests.Tools;

namespace RestWithAspNet10.IntegrationTests
{
    public class SwaggerIntegrationTests : IClassFixture<SqlServerFixture>
    {
        private readonly HttpClient _client;

        public SwaggerIntegrationTests(SqlServerFixture sqlServerFixture)
        {

            var factory = new CustomWebApplicationFactory<Program>(sqlServerFixture.ConnectionString);

            _client = factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    BaseAddress = new Uri("http://localhost")
                });
        }

        [Fact]
        public async Task GetSwaggerJson_ShouldReturnSwaggerDocument()
        {
            // Arrange
            var requestUri = "/swagger/v1/swagger.json";

            // Act
            var response = await _client.GetAsync(requestUri);

            // Assert
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            content.Should().NotBeNullOrEmpty();
            content.Should().Contain("\"openapi\": \"3.0.4\"");

        }

        [Fact]
        public async Task SwaggerUI_ShouldReturnSwaggerUI()
        {
            // Arrange
            var requestUri = "/swagger/index.html";

            // Act
            var response = await _client.GetAsync(requestUri);

            // Assert
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            content.Should().NotBeNullOrEmpty();
            content.Should().Contain("<title>ASP.NET 10 RESTful API with Swagger, whit Docker and Kubernetes</title>");
            content.Should().Contain("<div id=\"swagger-ui\">");


        }

    }
}
