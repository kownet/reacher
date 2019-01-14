using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Reacher.Destination.Facebook;
using Reacher.Publisher.App.DI;
using Reacher.Shared.Utils;
using Reacher.Storage.File.Json.Configuration;
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

                var servicesProvider = DependencyProvider.Get(configuration, clientId);

                if(ConfigurationExists(
                    servicesProvider.GetService<IOptions<StorageFileJsonConfiguration>>(),
                    clientId))
                {
                    servicesProvider.GetRequiredService<DestinationFacebookService>().Publish();
                }
            }

            NLog.LogManager.Shutdown();

            Console.ReadLine();
        }

        static bool ConfigurationExists(IOptions<StorageFileJsonConfiguration> jsonStorageConfiguration, string clientId)
            => File.Exists(Path.Combine(jsonStorageConfiguration.Value.Path, StorageFile.Name(clientId)));
    }
}