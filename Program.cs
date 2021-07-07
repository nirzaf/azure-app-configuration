using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace azure_app_configuration
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration(config =>
                    {
                        var appUri = config.Build().GetConnectionString("appConfiguration");
                        config.AddAzureAppConfiguration(options =>
                        {
                            var credential = new DefaultAzureCredential();
                            options.Connect(new Uri(appUri), credential);
                            options.ConfigureRefresh(refresh =>
                            {
                                refresh
                                    .Register("refreshAll", true)
                                    .SetCacheExpiration(TimeSpan.FromSeconds(5));
                            });
                        });

                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
