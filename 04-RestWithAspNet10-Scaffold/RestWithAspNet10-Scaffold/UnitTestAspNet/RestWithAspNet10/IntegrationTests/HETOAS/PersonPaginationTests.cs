using RestWithAspNet10.IntegrationTests.Fixtures;
using RestWithAspNet10.IntegrationTests.Tools;
using RestWithAspNet10_Scaffold.DTOs.Common;
using RestWithAspNet10_Scaffold.DTOs.V1.Person;
using System.Text.Json;

namespace RestWithAspNet10.IntegrationTests.HETOAS
{
    [Collection("LocalDbCollection")]
    public class PersonPaginationTests : IClassFixture<SqlServerFixture>
    {
        private readonly HttpClient _client;

        public PersonPaginationTests(LocalDbSqlFixture fixture)
        {
            var factory = new CustomWebApplicationFactory<Program>(fixture.ConnectionString);
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task ShouldReturnPagedPersons()
        {
            var response = await _client.GetAsync("/api/v1/person?page=1&pageSize=5");

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<PagedResponse<PersonResponseDTO>>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(result);

            Assert.True(result.Page > 0);
            Assert.True(result.PageSize > 0);
            Assert.True(result.TotalItems > 0);
            Assert.True(result.TotalPages > 1);
        }

    }
}
