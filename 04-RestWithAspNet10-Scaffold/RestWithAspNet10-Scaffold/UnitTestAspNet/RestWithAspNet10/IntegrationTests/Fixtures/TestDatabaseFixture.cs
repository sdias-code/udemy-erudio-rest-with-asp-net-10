using Microsoft.Data.SqlClient;
using Respawn;
using Respawn.Graph;

namespace RestWithAspNet10.IntegrationTests.Fixtures;

public class TestDatabaseFixture
{
    public string ConnectionString { get; private set; } = default!;
    private Respawner _respawner = default!;

    public async Task InitializeAsync(string connectionString)
    {
        ConnectionString = connectionString;

        await using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        _respawner = await Respawner.CreateAsync(connection, new RespawnerOptions
        {
            TablesToIgnore = new[]
            {
                new Table("__EFMigrationsHistory")
               
            },
            WithReseed = true
        });
    }

    public async Task ResetAsync()
    {
        await using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        await _respawner.ResetAsync(connection);
    }

    public async Task SeedAsync()
    {
        await using var conn = new SqlConnection(ConnectionString);
        await conn.OpenAsync();

        var cmd = conn.CreateCommand();
        cmd.CommandText = """
        INSERT INTO person (first_name, last_name, address, gender, enabled)
        VALUES 
        ('Ayrton','Senna','São Paulo - Brasil','Male',1),
        ('Marie','Curie','Paris','Female',1),
        ('Nikola','Tesla','Smiljan','Male',1);
        """;

        await cmd.ExecuteNonQueryAsync();
    }

}
