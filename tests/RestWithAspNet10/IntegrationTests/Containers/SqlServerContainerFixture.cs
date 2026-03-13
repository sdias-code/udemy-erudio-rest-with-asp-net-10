using Testcontainers.MsSql;

namespace RestWithAspNet10.IntegrationTests.Containers
{
    public class SqlServerContainerFixture : IAsyncLifetime
    {
        public MsSqlContainer Container { get; private set; }

        public async Task InitializeAsync()
        {
            Container = new MsSqlBuilder()
                .WithPassword("yourStrong(!)Password")
                .Build();

            await Container.StartAsync();
        }

        public async Task DisposeAsync()
        {
            await Container.DisposeAsync();
        }
    }
}
