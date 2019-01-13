using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reacher.Storage.Data.Models;
using Reacher.Storage.File.Json.Configuration;
using System;
using System.Collections.Generic;

namespace Reacher.Storage.File.Json
{
    public class StorageFileJsonService : IStorageFileJsonService
    {
        private readonly IOptions<StorageFileJsonConfiguration> _configuration;
        private readonly ILogger<StorageFileJsonService> _logger;

        public StorageFileJsonService(
            IOptions<StorageFileJsonConfiguration> configuration,
            ILogger<StorageFileJsonService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public IEnumerable<Content> GetAll()
        {
            var result = new List<Content>();

            try
            {

            }
            catch (Exception ex)
            {
                _logger.LogError($"Message: {ex.Message}");
            }

            return result;
        }

        public void Save(Content content)
        {
            try
            {

            }
            catch (Exception ex)
            {
                _logger.LogError($"Message: {ex.Message}");
            }
        }
    }
}