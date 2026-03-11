using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace RestWithAspNet10_Scaffold.Configurations
{
    public static class SerilogConfiguration
    {
        public static void ConfigureSerilog(this WebApplicationBuilder builder)
        {
            var conn = builder.Configuration.GetConnectionString("SqlServerConnection");

            Console.WriteLine("IConfiguration.GetConnectionString: " + conn);
            Console.WriteLine("ENV ConnectionStrings__SqlServerConnection: " +
                Environment.GetEnvironmentVariable("ConnectionStrings__SqlServerConnection"));

            var loggerConfig = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Debug();

            if (!string.IsNullOrWhiteSpace(conn))
            {
                var sinkOptions = new MSSqlServerSinkOptions
                {
                    TableName = "Logs",
                    AutoCreateSqlTable = false
                };

                loggerConfig = loggerConfig.WriteTo.MSSqlServer(conn, sinkOptions);
            }
            else
            {
                loggerConfig = loggerConfig.ReadFrom.Configuration(builder.Configuration);
            }

            Log.Logger = loggerConfig.CreateLogger();
            builder.Host.UseSerilog();
        }
    }
}
