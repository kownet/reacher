using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reacher.Destination.Facebook.Configuration;
using Reacher.Facebook.Api;
using Reacher.Notification.Pushover;
using Reacher.Shared.Utils;
using Reacher.Storage.File.Json;
using System;
using System.Threading.Tasks;

namespace Reacher.Destination.Facebook
{
    public class DestinationFacebookService
    {
        private readonly IOptions<DestinationFacebookConfiguration> _configuration;
        private readonly ILogger<DestinationFacebookService> _logger;
        private readonly INotificationPushoverService _pushoverNotification;
        private readonly IStorageFileJsonService _storageFileJson;
        private readonly IFacebookClient _facebookClient;
        private readonly IFacebookService _facebookService;

        public DestinationFacebookService(
            IOptions<DestinationFacebookConfiguration> configuration,
            ILogger<DestinationFacebookService> logger,
            INotificationPushoverService pushoverNotification,
            IStorageFileJsonService storageFileJson,
            IFacebookClient facebookClient,
            IFacebookService facebookService)
        {
            _configuration = configuration;
            _logger = logger;
            _pushoverNotification = pushoverNotification;
            _storageFileJson = storageFileJson;
            _facebookClient = facebookClient;
            _facebookService = facebookService;
        }

        public void Publish()
        {
            try
            {
                var postOnWallTask = _facebookService.PostOnPageAsync(
                    _configuration.Value.AccessKeys.AccessKey,
                    _configuration.Value.FanPage,
                    Titles.PublisherFacebookHeader);

                Task.WaitAll(postOnWallTask);

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