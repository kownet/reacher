using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reacher.Notification.Pushover;
using Reacher.Shared.Extensions;
using Reacher.Shared.Utils;
using Reacher.Source.Twitter.Configuration;
using Reacher.Storage.Data.Models;
using Reacher.Storage.File.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Reacher.Source.Twitter
{
    public class SourceTwitterService
    {
        private readonly IOptions<SourceTwitterConfiguration> _configuration;
        private readonly ILogger<SourceTwitterService> _logger;
        private readonly INotificationPushoverService _pushoverNotification;
        private readonly IStorageFileJsonService _storageFileJson;

        private readonly ITwitterCredentials _twitterCredentials; 

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

            _twitterCredentials = Auth.CreateCredentials(
                _configuration.Value.AccessKeys.ConsumerKey,
                _configuration.Value.AccessKeys.ConsumerSecret,
                _configuration.Value.AccessKeys.UserAccessToken,
                _configuration.Value.AccessKeys.UserAccessSecret);
        }

        public void Collect()
        {
            var newContent = new Content();

            Auth.ExecuteOperationWithCredentials(_twitterCredentials, () =>
            {
                var timeline = Timeline.GetUserTimeline(_configuration.Value.Accounts.First());

                if(timeline.AnyAndNotNull())
                {
                    var firstTweet = timeline.First();

                    newContent.Id = firstTweet.IdStr;
                    newContent.Message = firstTweet.Text;
                }
            });

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
            if(newContent.IsProvided)
            {
                _storageFileJson.Save(newContent);

                var message = $"{newContent.ToString()} stored for publish.";

                _logger.LogInformation(message);

                _pushoverNotification.Send(Titles.CollectorTwitterHeader, message);
            }
        }
    }
}