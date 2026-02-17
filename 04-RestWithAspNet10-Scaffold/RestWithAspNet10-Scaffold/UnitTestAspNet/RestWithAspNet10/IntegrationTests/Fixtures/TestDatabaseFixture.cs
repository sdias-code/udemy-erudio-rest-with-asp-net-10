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

    public async Task SeedPersonAsync()
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

    public async Task SeedBookAsync()
    {
        await using var conn = new SqlConnection(ConnectionString);
        await conn.OpenAsync();

        var cmd = conn.CreateCommand();
        cmd.CommandText = """
        INSERT INTO books (title, author, price, launch_date)
        VALUES
        ('Clean Code', 'Robert C. Martin', 50.00, '2008-08-01'),
        ('The Pragmatic Programmer', 'Andrew Hunt', 45.00, '1999-10-30'),
        ('Design Patterns', 'Erich Gamma', 60.00, '1994-10-21');
        """;

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task SeedAllAsync()
    {
        await SeedPersonAsync();
        await SeedBookAsync();
    }
}
