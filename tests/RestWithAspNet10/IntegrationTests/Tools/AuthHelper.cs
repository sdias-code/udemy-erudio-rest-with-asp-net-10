using RestWithAspNet10_Scaffold.DTOs.V1.Token;
using RestWithAspNet10_Scaffold.DTOs.V1.User;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace RestWithAspNet10.IntegrationTests.Tools;

public static class AuthHelper
{
    public static async Task AuthenticateAsync(
        this HttpClient client)
    {
        // Login
        var response = await client.PostAsJsonAsync(
            "/api/auth/signin",
            new UserDTO
            {
                Username = "testuser",
                Password = "123456"
            });

        response.EnsureSuccessStatusCode();

        var token = await response.Content
            .ReadFromJsonAsync<TokenDTO>();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                token!.AccessToken);
    }
}