using Microsoft.Data.SqlClient;
using Respawn;

namespace RestWithAspNet10.IntegrationTests.Fixtures
{
    public static class DatabaseFixture
    {
        private static Respawner? _respawner;

        public static async Task InitializeAsync(string connectionString)
        {
            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            _respawner = await Respawner.CreateAsync(connection, new RespawnerOptions
            {
                TablesToIgnore = new[] { new Respawn.Graph.Table("__EFMigrationsHistory") }
            });
        }

        public static async Task ResetAsync(string connectionString)
        {
            if (_respawner is null)
            {
                await InitializeAsync(connectionString);
            }

            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            await _respawner!.ResetAsync(connection);
        }

    }
}
