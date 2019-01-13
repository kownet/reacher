using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Reacher.Destination.Facebook;
using Reacher.Destination.Facebook.Configuration;
using Reacher.Notification.Pushover;
using Reacher.Notification.Pushover.Configuration;
using System;

namespace Reacher.Publisher.App.DI
{
    public static class DependencyProvider
    {
        public static IServiceProvider Get(IConfigurationRoot configurationRoot)
        {
            var services = new ServiceCollection();

            #region Logging
            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Trace);
                builder.AddNLog(new NLogProviderOptions
                {
                    CaptureMessageTemplates = true,
                    CaptureMessageProperties = true
                });
            });
            #endregion

            #region Destinations
            services.Configure<DestinationFacebookConfiguration>(
                opt => configurationRoot
                .GetSection("destinations:facebook")
                .Bind(opt));

            services.AddTransient<DestinationFacebookService>();
            #endregion

            #region Notifications
            services.Configure<NotificationPushoverConfiguration>(
                opt => configurationRoot
                .GetSection("notifications:pushover")
                .Bind(opt));

            services.AddTransient<INotificationPushoverService, NotificationPushoverService>();
            #endregion

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}