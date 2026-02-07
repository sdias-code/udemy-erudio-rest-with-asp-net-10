using EvolveDb;
using Microsoft.Data.SqlClient;
using Serilog;

namespace RestWithAspNet10_Scaffold.Configurations
{
    public static class EvolveConfig
    {
        private static readonly List<string> DefaultLocations = new()
        {
            "db/migrations",
            "db/dataset"
        };

        public static void ExecuteMigrations(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            try
            {
                using var connection = new SqlConnection(connectionString);

                var evolve = new Evolve(
                    connection,
                    msg =>
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

        // Opcional: usar no Program.cs se quiser
        public static IServiceCollection AddEvolveConfiguration(
            this IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            if (!environment.IsDevelopment())
                return services;

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            ExecuteMigrations(connectionString!);

            return services;
        }
    }
}
