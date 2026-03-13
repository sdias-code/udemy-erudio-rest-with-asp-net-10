using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace RestWithAspNet10.IntegrationTests;

public class CorsIntegrationTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly IConfiguration _configuration;

    public CorsIntegrationTests(
        WebApplicationFactory<Program> factory)
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

        var request = new HttpRequestMessage(
            HttpMethod.Options, "/api/v2/person");

        request.Headers.Add("Origin", origin);
        request.Headers.Add(
            "Access-Control-Request-Method", "GET");

        var response = await _client.SendAsync(request);

        response.StatusCode.Should()
            .Be(HttpStatusCode.NoContent);

        response.Headers.Should()
            .ContainKey("Access-Control-Allow-Origin");

        var corsHeader = response.Headers
            .GetValues("Access-Control-Allow-Origin")
            .First();

        corsHeader.Should().Be(origin);
    }
}