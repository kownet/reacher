using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reacher.Notification.Pushover;
using Reacher.Shared.Utils;
using Reacher.Source.Twitter.Configuration;
using Reacher.Storage.Data.Models;
using Reacher.Storage.File.Json;
using System;
using System.Linq;

namespace Reacher.Source.Twitter
{
    public class SourceTwitterService
    {
        private readonly IOptions<SourceTwitterConfiguration> _configuration;
        private readonly ILogger<SourceTwitterService> _logger;
        private readonly INotificationPushoverService _pushoverNotification;
        private readonly IStorageFileJsonService _storageFileJson;

        public SourceTwitterService(
            IOptions<SourceTwitterConfiguration> configuration,
            ILogger<SourceTwitterService> logger,
            INotificationPushoverService pushoverNotification,
            IStorageFileJsonService storageFileJson)
        {
            _configuration = configuration;
            _logger = logger;
            _pushoverNotification = pushoverNotification;
            _storageFileJson = storageFileJson;
        }

        public void Collect()
        {
            var newContent = new Content("test-id", "test-message");

            try
            {
                var allContent = _storageFileJson.GetAll();

                if (!allContent.Any())
                {
                    Store(newContent);
                }
                else
                {
                    if (!allContent.Any(c => string.Equals(c.Id, newContent.Id)))
                    {
                        Store(newContent);
                    }
                    else
                    {
                        _logger.LogInformation($"{newContent.ToString()} already prepared for publish.");
                    }
                }
            }
            catch (Exception)
            {
                var message = $"{newContent.ToString()} not stored for publish.";

                _logger.LogError(message);

                _pushoverNotification.Send(Titles.CollectorTwitterHeader, message);
            }
        }

        private void Store(Content newContent)
        {
            _storageFileJson.Save(newContent);

            var message = $"{newContent.ToString()} stored for publish.";

            _logger.LogInformation(message);

            _pushoverNotification.Send(Titles.CollectorTwitterHeader, message);
        }
    }
}