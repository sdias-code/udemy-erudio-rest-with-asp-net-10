using RestWithAspNet10.IntegrationTests.Base;
using RestWithAspNet10.IntegrationTests.Fixtures;
using RestWithAspNet10.IntegrationTests.Tools;
using RestWithAspNet10_Scaffold.DTOs.Common;
using RestWithAspNet10_Scaffold.DTOs.V1.Person;
using System.Text.Json;

namespace RestWithAspNet10.IntegrationTests.HETOAS
{
    [Collection("IntegrationTests")]
    public class PersonPaginationTests
        : AuthenticatedIntegrationTest
    {
        public PersonPaginationTests(
            SqlServerFixture sqlServerFixture,
            TestDatabaseFixture db)
            : base(sqlServerFixture, db)
        {
        }

        [Fact]
        public async Task ShouldReturnPagedPersons()
        {
            await SetupAsync();

            var response = await _client
                .GetAsync("/api/v1/person?page=1&pageSize=5");

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<PagedResponse<PersonResponseDTO>>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(result);
            Assert.True(result.Page > 0);
            Assert.True(result.PageSize > 0);
            Assert.True(result.TotalItems > 0);
            Assert.True(result.TotalPages > 0);
        }
    }
}