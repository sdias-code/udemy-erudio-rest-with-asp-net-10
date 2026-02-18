using Microsoft.Data.SqlClient;
using RestWithAspNet10_Scaffold.Configurations;

namespace RestWithAspNet10.IntegrationTests.Fixtures
{
 
        public class LocalDbSqlFixture : IAsyncLifetime
        {
            public string ConnectionString { get; } =
                @"Server=(localdb)\MSSQLLocalDB;
                Database=db_erudio;
                Trusted_Connection=True;
                 MultipleActiveResultSets=true";

            public async Task InitializeAsync()
            {
                Console.WriteLine("=================================");
                Console.WriteLine("LOCALDB CONNECTION:");
                Console.WriteLine(ConnectionString);
                Console.WriteLine("=================================");

                ValidateConnection(ConnectionString);

                // garante schema sempre atualizado
                EvolveConfig.ExecuteMigrations(ConnectionString);

                await Task.CompletedTask;
            }

            public async Task DisposeAsync()
            {
                await Task.CompletedTask;
            }

            private void ValidateConnection(string conn)
            {
                using var connection = new SqlConnection(conn);
                connection.Open();
                Console.WriteLine("LocalDB connected successfully.");
            }
        }
}
