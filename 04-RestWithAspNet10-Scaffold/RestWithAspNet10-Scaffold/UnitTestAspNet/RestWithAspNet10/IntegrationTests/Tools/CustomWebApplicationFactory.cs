using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

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
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    var configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .AddEnvironmentVariables()
                        .Build();
    
                    // Override the connection string with the one provided in the constructor
                    configuration["ConnectionStrings:DefaultConnection"] = _conectionString;

                    config.AddConfiguration(configuration);

                    //config.AddInMemoryCollection(configuration!);
                });
        }

    }
}
