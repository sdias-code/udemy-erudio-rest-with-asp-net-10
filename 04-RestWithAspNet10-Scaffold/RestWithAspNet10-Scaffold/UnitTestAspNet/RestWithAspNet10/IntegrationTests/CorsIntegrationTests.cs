using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace RestWithAspNet10.IntegrationTests
{
    public class CorsIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;

        public CorsIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();

            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        [Fact]
        public async Task Should_allow_request_from_allowed_origin()
        {
            var allowedOrigins = _configuration
                .GetSection("Cors:Origins")
                .Get<string[]>();

            var origin = allowedOrigins!.First();

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v2/person");

            request.Headers.Add("Origin", origin);

            var response = await _client.SendAsync(request);

            Assert.True(
                response.Headers.Contains("Access-Control-Allow-Origin"));

            var corsHeader = response.Headers
                .GetValues("Access-Control-Allow-Origin")
                .First();

            Assert.Equal(origin, corsHeader);
        }


    }
}
