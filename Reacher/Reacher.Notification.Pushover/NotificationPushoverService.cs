using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reacher.Notification.Pushover.Configuration;
using System;
using System.Collections.Specialized;
using System.Net;

namespace Reacher.Notification.Pushover
{
    public class NotificationPushoverService : INotificationPushoverService
    {
        private readonly IOptions<NotificationPushoverConfiguration> _configuration;
        private readonly ILogger<NotificationPushoverService> _logger;

        public NotificationPushoverService(
            IOptions<NotificationPushoverConfiguration> configuration,
            ILogger<NotificationPushoverService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public void Send(string title, string message)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(message))
                {
                    var parameters = new NameValueCollection
                {
                    { "token", _configuration.Value.Token },
                    { "user", _configuration.Value.Recipients },
                    { "message", message },
                    { "title", title }
                };

                    using (var client = new WebClient())
                    {
                        client.UploadValues(_configuration.Value.Endpoint, parameters);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Message: {ex.Message}");
            }
        }
    }
}