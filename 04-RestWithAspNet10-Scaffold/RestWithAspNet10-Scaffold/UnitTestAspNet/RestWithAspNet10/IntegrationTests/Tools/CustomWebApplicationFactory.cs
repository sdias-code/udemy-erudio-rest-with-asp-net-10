using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestWithAspNet10_Scaffold.Data;

namespace RestWithAspNet10.IntegrationTests.Tools
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        private readonly string _conectionString;

        public CustomWebApplicationFactory(string conectionString)
        {
            _conectionString = conectionString;
        }

            protected override void ConfigureWebHost(IWebHostBuilder builder)
            {
            builder.ConfigureServices(services =>
            {
                // Remove o DbContext real
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                if (descriptor != null)
                    services.Remove(descriptor);

                // Registra usando o container
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseSqlServer(_conectionString);
                });
            });
        }

    }
}
