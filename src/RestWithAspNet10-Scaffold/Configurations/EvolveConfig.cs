using EvolveDb;
using Microsoft.Data.SqlClient;
using Serilog;
using System.Data.Common;

namespace RestWithAspNet10_Scaffold.Configurations
{
    public static class EvolveConfig
    {
        private static readonly List<string> DefaultLocations = new()
        {
            "db/migrations",
            "db/dataset"
        };

        public static void ExecuteMigrations(IConfiguration configuration)
        {
            var connString = configuration.GetConnectionString("SqlServerConnection");

            DbConnection connection = new SqlConnection(connString);

            const int maxRetries = 10;
            int retries = 0;

            while (true)
            {
                try
                {
                    connection.Open();
                    break;
                }
                catch
                {
                    retries++;

                    if (retries >= maxRetries)
                        throw;

                    Console.WriteLine($"[EVOLVE] Waiting database... retry {retries}");
                    Thread.Sleep(5000);
                }
            }

            try
            {
                var evolve = new Evolve(connection, msg =>
                {
                    Console.WriteLine($"[EVOLVE] {msg}");
                    Log.Information(msg);
                })
                {
                    Locations = DefaultLocations,
                    IsEraseDisabled = true,
                    CommandTimeout = 60
                };

                evolve.Migrate();

                Console.WriteLine("[EVOLVE] Migrations executed successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[EVOLVE] Migration failed");
                Log.Error(ex, "Evolve migration failed");
                throw;
            }
        }
    }
}