using RestWithAspNet10_Scaffold.Configurations;
using Testcontainers.MsSql;

namespace RestWithAspNet10.IntegrationTests.Tools
{
    public class SqlServerFixture : IAsyncLifetime
    {
        public MsSqlContainer Container { get;  }
        public string ConnectionString => Container.GetConnectionString();
        public SqlServerFixture()
        {
            Container = new MsSqlBuilder()                
                .Build();
        }
        public async Task InitializeAsync()
        {
            await Container.StartAsync();

            EvolveConfig.ExecuteMigrations(ConnectionString);
        }
        public async Task DisposeAsync()
        {
            await Container.DisposeAsync();
        }
    }
}
