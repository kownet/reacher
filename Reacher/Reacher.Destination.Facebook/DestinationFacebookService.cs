using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reacher.Destination.Facebook.Configuration;
using Reacher.Notification.Pushover;
using Reacher.Storage.File.Json;

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
            var message = $"Fanpage to publish: {_configuration.Value.FanPage}";

            _logger.LogInformation(message);
            _pushoverNotification.Send("Publisher Facebook Destination", message);
        }
    }
}