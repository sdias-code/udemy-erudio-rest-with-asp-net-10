using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using RestWithAspNet10.IntegrationTests.Fixtures;
using RestWithAspNet10.IntegrationTests.Tools;
using RestWithAspNet10_Scaffold.DTOs.V1.Token;
using RestWithAspNet10_Scaffold.DTOs.V1.User;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

[Collection("IntegrationTests")]
public class AuthControllerIntegrationTests
    : IClassFixture<SqlServerFixture>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly TestDatabaseFixture _db;

    public AuthControllerIntegrationTests(
        SqlServerFixture sqlServerFixture,
        TestDatabaseFixture db)
    {
        _factory = new CustomWebApplicationFactory<Program>(
            sqlServerFixture.ConnectionString);

        _db = db;

        _db.InitializeAsync(sqlServerFixture.ConnectionString)
            .GetAwaiter().GetResult();
    }

    [Fact]
    public async Task SignIn_Should_Return_Token()
    {
        var client = _factory.CreateClient();

        await _db.ResetAsync();
        await _db.SeedUserAsync();

        var response = await client.PostAsJsonAsync(
            "/api/auth/signin",
            new UserDTO
            {
                Username = "testuser",
                Password = "123456"
            });

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var token = await response.Content
            .ReadFromJsonAsync<TokenDTO>();

        token.Should().NotBeNull();
        token!.AccessToken.Should().NotBeNullOrEmpty();
        token.RefreshToken.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Refresh_Should_Return_New_Token_When_Valid()
    {
        var client = _factory.CreateClient();

        await _db.ResetAsync();
        await _db.SeedUserAsync();

        var loginResponse = await client.PostAsJsonAsync(
            "/api/auth/signin",
            new UserDTO
            {
                Username = "testuser",
                Password = "123456"
            });

        loginResponse.EnsureSuccessStatusCode();

        var loginToken = await loginResponse.Content
            .ReadFromJsonAsync<TokenDTO>();

        loginToken.Should().NotBeNull();
        loginToken!.RefreshToken.Should().NotBeNullOrEmpty();

        var refreshResponse = await client.PostAsJsonAsync(
            "/api/auth/refresh",
            new TokenDTO
            {
                AccessToken = loginToken.AccessToken,
                RefreshToken = loginToken.RefreshToken
            });

        refreshResponse.EnsureSuccessStatusCode();

        var newToken = await refreshResponse.Content
            .ReadFromJsonAsync<TokenDTO>();

        newToken.Should().NotBeNull();
        newToken!.AccessToken.Should().NotBe(loginToken.AccessToken);
    }

    [Fact]
    public async Task Revoke_Should_Invalidate_RefreshToken()
    {
        var client = _factory.CreateClient();

        await _db.ResetAsync();
        await _db.SeedUserAsync();

        var loginResponse = await client.PostAsJsonAsync(
            "/api/auth/signin",
            new UserDTO
            {
                Username = "testuser",
                Password = "123456"
            });

        loginResponse.EnsureSuccessStatusCode();

        var token = await loginResponse.Content
            .ReadFromJsonAsync<TokenDTO>();

        token.Should().NotBeNull();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                token!.AccessToken);

        var revokeResponse =
            await client.PostAsync("/api/auth/revoke", null);

        revokeResponse.StatusCode
            .Should().Be(HttpStatusCode.NoContent);

        var refreshResponse = await client.PostAsJsonAsync(
            "/api/auth/refresh",
            new TokenDTO
            {
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken
            });

        refreshResponse.StatusCode
            .Should().Be(HttpStatusCode.Unauthorized);
    }
}