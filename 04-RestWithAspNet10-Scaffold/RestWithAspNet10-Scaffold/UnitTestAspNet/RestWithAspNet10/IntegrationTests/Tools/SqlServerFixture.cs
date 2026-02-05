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
            Container = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-CU14-ubuntu-22.04")
                .WithPassword("@Admin123")
                .Build();
            Container.StartAsync().GetAwaiter().GetResult();
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
