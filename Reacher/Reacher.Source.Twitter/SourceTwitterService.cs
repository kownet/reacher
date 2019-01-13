using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reacher.Notification.Pushover;
using Reacher.Source.Twitter.Configuration;

namespace Reacher.Source.Twitter
{
    public class SourceTwitterService
    {
        private readonly IOptions<SourceTwitterConfiguration> _configuration;
        private readonly ILogger<SourceTwitterService> _logger;
        private readonly INotificationPushoverService _pushoverNotification;

        public SourceTwitterService(
            IOptions<SourceTwitterConfiguration> configuration,
            ILogger<SourceTwitterService> logger,
            INotificationPushoverService pushoverNotification)
        {
            _configuration = configuration;
            _logger = logger;
            _pushoverNotification = pushoverNotification;
        }

        public void Collect()
        {
            var message =
                $"Hashtags to monitor: '{string.Join(",", _configuration.Value.Hashtags)}'"
                + $" on: '{string.Join(",", _configuration.Value.Accounts)}' accounts";

            _logger.LogInformation(message);
            _pushoverNotification.Send("Collector Twitter Source", message);
        }
    }
}