using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Reacher.Collector.App.DI;
using Reacher.Shared.Utils;
using Reacher.Source.Twitter;
using Reacher.Storage.File.Json;
using Reacher.Storage.File.Json.Configuration;
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

                var servicesProvider = DependencyProvider.Get(configuration, clientId);

                Create(
                    servicesProvider.GetService<IStorageFileJsonService>(),
                    servicesProvider.GetService<IOptions<StorageFileJsonConfiguration>>(),
                    clientId);

                servicesProvider.GetRequiredService<SourceTwitterService>().Collect();
            }

            NLog.LogManager.Shutdown();
        }

        static void Create(IStorageFileJsonService jsonStorage, IOptions<StorageFileJsonConfiguration> jsonStorageConfiguration, string clientId)
        {
            if (jsonStorage != null && jsonStorageConfiguration != null)
            {
                var jsonStorageDir = Path.Combine(jsonStorageConfiguration.Value.Path);

                if (!Directory.Exists(jsonStorageDir))
                {
                    Directory.CreateDirectory(jsonStorageDir);
                }

                var jsonStorageFile = Path.Combine(jsonStorageConfiguration.Value.Path, StorageFile.Name(clientId));

                if (!File.Exists(jsonStorageFile))
                {
                    File.Create(jsonStorageFile).Dispose();
                }
            }
        }
    }
}