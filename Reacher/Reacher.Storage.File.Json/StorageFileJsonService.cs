using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Reacher.Shared.Common;
using Reacher.Shared.Utils;
using Reacher.Storage.Data.Models;
using Reacher.Storage.File.Json.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace Reacher.Storage.File.Json
{
    public class StorageFileJsonService : IStorageFileJsonService
    {
        private readonly IOptions<StorageFileJsonConfiguration> _configuration;
        private readonly ILogger<StorageFileJsonService> _logger;
        private readonly IClientProvider _client;

        public StorageFileJsonService(
            IOptions<StorageFileJsonConfiguration> configuration,
            ILogger<StorageFileJsonService> logger,
            IClientProvider client)
        {
            _configuration = configuration;
            _logger = logger;
            _client = client;
        }

        public IEnumerable<Content> GetAll()
        {
            var result = new List<Content>();

            try
            {
                var currentContent = System.IO.File.ReadAllText(
                    Path.Combine(_configuration.Value.Path, StorageFile.Name(_client.ClientId())));

                var currentContentList = JsonConvert.DeserializeObject<Content>(currentContent);

                if(currentContentList != null)
                {
                    result.Add(currentContentList);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Message: {ex.Message}");

                throw ex;
            }

            return result;
        }

        public Content GetLatest()
        {
            try
            {
                var currentContent = System.IO.File.ReadAllText(
                    Path.Combine(_configuration.Value.Path, StorageFile.Name(_client.ClientId())));

                var currentContentObject = JsonConvert.DeserializeObject<Content>(currentContent);

                return currentContentObject;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Message: {ex.Message}");

                throw ex;
            }
        }

        public void Save(Content content)
        {
            try
            {
                var json = JsonConvert.SerializeObject(content);

                System.IO.File.WriteAllText(
                    Path.Combine(_configuration.Value.Path, StorageFile.Name(_client.ClientId())),
                    json);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Message: {ex.Message}");

                throw ex;
            }
        }
    }
}