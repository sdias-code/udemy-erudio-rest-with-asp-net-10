using RestWithAspNet10.IntegrationTests.Fixtures;
using RestWithAspNet10.IntegrationTests.Tools;

namespace RestWithAspNet10.IntegrationTests.HETOAS
{
    [Collection("IntegrationTests")]
    public class BookControllerHATOASTests : IClassFixture<SqlServerFixture>
    {
        private readonly HttpClient _client;
        private readonly TestDatabaseFixture _db;

        public BookControllerHATOASTests(SqlServerFixture sqlServerFixture, TestDatabaseFixture db)
        {
            var factory = new CustomWebApplicationFactory<Program>
                (sqlServerFixture.ConnectionString);

            _db = db;

            _db.InitializeAsync(sqlServerFixture.ConnectionString)
                .GetAwaiter().GetResult();

            _client = factory.CreateClient();
        }

    }
}
