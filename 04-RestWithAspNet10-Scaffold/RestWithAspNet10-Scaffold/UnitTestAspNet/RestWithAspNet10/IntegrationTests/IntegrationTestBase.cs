using RestWithAspNet10.IntegrationTests.Fixtures;

namespace RestWithAspNet10.IntegrationTests
{
    public class IntegrationTestBase : IAsyncLifetime
    {
        protected readonly TestDatabaseFixture DbFixture;

        public IntegrationTestBase()
        {
            DbFixture = new TestDatabaseFixture();
        }

        public async Task InitializeAsync()
        {
            // Conecta no banco do testcontainer e aplica respawner
            await DbFixture.InitializeAsync("Server=127.0.0.1,1433;Database=testdb;User Id=sa;Password=Admin@123;TrustServerCertificate=True");

            // Reseta e popula todas as tabelas
            await DbFixture.ResetAsync();
            await DbFixture.SeedAllAsync();
        }

        public async Task DisposeAsync()
        {
            // Opcional: resetar banco após os testes
            await DbFixture.ResetAsync();
        }
    }
}
