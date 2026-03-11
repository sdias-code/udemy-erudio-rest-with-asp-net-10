using RestWithAspNet10.IntegrationTests.Fixtures;
using RestWithAspNet10.IntegrationTests.Tools;

namespace RestWithAspNet10.IntegrationTests.Base
{
    public abstract class AuthenticatedIntegrationTest
    : IClassFixture<SqlServerFixture>
    {
        protected readonly HttpClient _client;
        protected readonly TestDatabaseFixture _db;

        protected AuthenticatedIntegrationTest(
            SqlServerFixture sqlServerFixture,
            TestDatabaseFixture db)
        {
            var factory = new CustomWebApplicationFactory<Program>(
                sqlServerFixture.ConnectionString);

            _db = db;

            _db.InitializeAsync(sqlServerFixture.ConnectionString)
                .GetAwaiter().GetResult();

            _client = factory.CreateClient();
        }

        protected async Task SetupAsync()
        {
            await _db.ResetAsync();
            await _db.SeedAllAsync(); // já inclui usuário
            await _client.AuthenticateAsync();
        }
    }

}
