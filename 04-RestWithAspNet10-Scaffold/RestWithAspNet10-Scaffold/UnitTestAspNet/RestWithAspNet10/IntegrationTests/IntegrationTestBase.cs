using RestWithAspNet10.IntegrationTests.Fixtures;
using RestWithAspNet10.IntegrationTests.Tools;

namespace RestWithAspNet10.IntegrationTests
{
    public abstract class IntegrationTestBase
    : IClassFixture<SqlServerFixture>
    {
        protected readonly CustomWebApplicationFactory<Program> _factory;
        protected readonly HttpClient _client;

        protected IntegrationTestBase(SqlServerFixture sqlFixture)
        {
            _factory = new CustomWebApplicationFactory<Program>(
                sqlFixture.ConnectionString);

            _client = _factory.CreateClient();
        }
    }
}
