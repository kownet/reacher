using Microsoft.Extensions.Options;
using Reacher.Storage.File.Json;
using Reacher.Storage.File.Json.Configuration;
using System.IO;

namespace Reacher.Shared.Utils
{
    public static class StorageFile
    {
        public static bool ConfigurationExists(IOptions<StorageFileJsonConfiguration> jsonStorageConfiguration, string clientId)
            => File.Exists(Path.Combine(jsonStorageConfiguration.Value.Path, $"storage.{clientId}.json"));

        public static void Create(IStorageFileJsonService jsonStorage, IOptions<StorageFileJsonConfiguration> jsonStorageConfiguration, string clientId)
        {
            if (jsonStorage != null && jsonStorageConfiguration != null)
            {
                var jsonStorageDir = Path.Combine(jsonStorageConfiguration.Value.Path);

                if (!Directory.Exists(jsonStorageDir))
                {
                    Directory.CreateDirectory(jsonStorageDir);
                }

                var jsonStorageFile = Path.Combine(jsonStorageConfiguration.Value.Path, $"storage.{clientId}.json");

                if (!File.Exists(jsonStorageFile))
                {
                    File.Create(jsonStorageFile);
                }
            }
        }
    }
}