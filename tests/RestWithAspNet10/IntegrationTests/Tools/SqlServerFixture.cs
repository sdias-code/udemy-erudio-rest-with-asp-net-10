//using Microsoft.Data.SqlClient;
//using RestWithAspNet10_Scaffold.Configurations;
//using Testcontainers.MsSql;


//namespace RestWithAspNet10.IntegrationTests.Tools
//{
//    public class SqlServerFixture : IAsyncLifetime
//    {   

//        private readonly MsSqlContainer _container;

//        public string ConnectionString => _container.GetConnectionString();

//        public SqlServerFixture()
//        {
//            _container = new MsSqlBuilder()
//                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
//                .WithPassword("Admin@123")
//                //.WithReuse(true)
//                //.WithPortBinding(1433, true)
//                .Build();
//        }
//        public async Task InitializeAsync()
//        {
//            await _container.StartAsync();

//            var conn = _container.GetConnectionString();

//            Console.WriteLine("=================================");
//            Console.WriteLine("TESTCONTAINER CONNECTION:");
//            Console.WriteLine(conn);
//            Console.WriteLine("=================================");

//            EvolveConfig.ExecuteMigrations(conn);

//            ValidateConnection(conn);
//        }
//        public async Task DisposeAsync()
//        {
//            await _container.DisposeAsync();
//        }

//        public void ValidateConnection(string conn)
//        {
//            try
//            {
//                using var connection = new SqlConnection(conn);
//                connection.Open();
//                Console.WriteLine("Connection to SQL Server successful.");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Failed to connect to SQL Server: {ex.Message}");
//                throw;
//            }
//        }
//    }
//}
