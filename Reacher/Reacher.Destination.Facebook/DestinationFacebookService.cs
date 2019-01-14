using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reacher.Destination.Facebook.Configuration;
using Reacher.Notification.Pushover;
using Reacher.Shared.Utils;
using Reacher.Storage.File.Json;
using System;

namespace Reacher.Destination.Facebook
{
    public class DestinationFacebookService
    {
        private readonly IOptions<DestinationFacebookConfiguration> _configuration;
        private readonly ILogger<DestinationFacebookService> _logger;
        private readonly INotificationPushoverService _pushoverNotification;
        private readonly IStorageFileJsonService _storageFileJson;

        public DestinationFacebookService(
            IOptions<DestinationFacebookConfiguration> configuration,
            ILogger<DestinationFacebookService> logger,
            INotificationPushoverService pushoverNotification,
            IStorageFileJsonService storageFileJson)
        {
            _configuration = configuration;
            _logger = logger;
            _pushoverNotification = pushoverNotification;
            _storageFileJson = storageFileJson;
        }

        public void Publish()
        {
            try
            {
                var latestContent = _storageFileJson.GetLatest();

                if(latestContent != null)
                {

                    var message = $"{latestContent.ToString()} published.";

                    _logger.LogInformation(message);

                    _pushoverNotification.Send(Titles.PublisherFacebookHeader, message);
                }
                else
                {
                    var message = $"There is no content to publish.";

                    _logger.LogInformation(message);

                    _pushoverNotification.Send(Titles.PublisherFacebookHeader, message);
                }
            }
            catch (Exception)
            {
                var message = $"Latest content not published.";

                _logger.LogError(message);

                _pushoverNotification.Send(Titles.PublisherFacebookHeader, message);
            }
        }
    }
}