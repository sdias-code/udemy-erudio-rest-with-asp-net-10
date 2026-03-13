using FluentAssertions;
using RestWithAspNet10.IntegrationTests.Containers;
using System.Net;
using System.Net.Http.Json;

namespace RestWithAspNet10.IntegrationTests.Controllers
{
    public class AuthControllerTests :
     IClassFixture<SqlServerContainerFixture>
    {
        private readonly HttpClient _client;

        public AuthControllerTests(SqlServerContainerFixture fixture)
        {
            var factory = new CustomWebApplicationFactory(
                fixture.Container.GetConnectionString());

            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Should_Create_User_And_Login()
        {
            var user = new
            {
                Username = "sdias",
                Password = "Admin123",
                FullName = "Silvio dias Ferreira"
            };

            var createResponse = await _client.PostAsJsonAsync(
                "/api/auth/create", user);

            createResponse.StatusCode
                .Should().Be(HttpStatusCode.OK);

            var loginResponse = await _client.PostAsJsonAsync(
                "/api/auth/signin", user);

            loginResponse.StatusCode
                .Should().Be(HttpStatusCode.OK);
        }
    }
}
