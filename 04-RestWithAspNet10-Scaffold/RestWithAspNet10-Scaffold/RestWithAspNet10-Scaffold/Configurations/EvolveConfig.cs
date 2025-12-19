using EvolveDb;
using Microsoft.Data.SqlClient;
using Serilog;

namespace RestWithAspNet10_Scaffold.Configurations
{
    public static class EvolveConfig
    {
        public static IServiceCollection AddEvolveConfiguration(
            this IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new ArgumentNullException(
                        "Connection string 'ConnectionString' not found.");
                }

                try
                {
                    using var evolveConnection = new SqlConnection(connectionString);

                    var evolve = new Evolve(
                        evolveConnection,
                        msg => Log.Information(msg))
                    {
                        Locations = new List<string> { "db/migrations", "db/dataset" },
                        IsEraseDisabled = true
                    };
                    evolve.Migrate();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "An error occurred while migrating the database.");
                    throw;
                }
            }
            return services;
        }
    }


}

