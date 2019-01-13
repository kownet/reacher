using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reacher.Collector.App.DI;
using Reacher.Source.Twitter;
using System;
using System.IO;

namespace Reacher.Collector.App
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                var clientId = args[0];

                NLog.LogManager.Configuration.Variables["fileName"] = $"reacher-collector-{clientId}-{DateTime.UtcNow.ToString("ddMMyyyy")}.log";
                NLog.LogManager.Configuration.Variables["archiveFileName"] = $"reacher-collector-{clientId}-{DateTime.UtcNow.ToString("ddMMyyyy")}.log";

                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile($"appsettings.{clientId}.json");

                var configuration = builder.Build();

                var servicesProvider = DependencyProvider.Get(configuration);

                servicesProvider.GetRequiredService<SourceTwitterService>().Collect();
            }
            else
            {

            }

            NLog.LogManager.Shutdown();

            Console.ReadLine();
        }
    }
}