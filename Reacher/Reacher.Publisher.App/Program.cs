using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reacher.Destination.Facebook;
using Reacher.Publisher.App.DI;
using System;
using System.IO;

namespace Reacher.Publisher.App
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                var clientId = args[0];

                NLog.LogManager.Configuration.Variables["fileName"] = $"reacher-publisher-{clientId}-{DateTime.UtcNow.ToString("ddMMyyyy")}.log";
                NLog.LogManager.Configuration.Variables["archiveFileName"] = $"reacher-publisher-{clientId}-{DateTime.UtcNow.ToString("ddMMyyyy")}.log";

                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile($"appsettings.{clientId}.json");

                var configuration = builder.Build();

                var servicesProvider = DependencyProvider.Get(configuration);

                servicesProvider.GetRequiredService<DestinationFacebookService>().Publish();
            }
            else
            {

            }

            NLog.LogManager.Shutdown();

            Console.ReadLine();
        }
    }
}