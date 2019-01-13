using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Reacher.Notification.Pushover;
using Reacher.Notification.Pushover.Configuration;
using Reacher.Source.Twitter;
using Reacher.Source.Twitter.Configuration;
using System;

namespace Reacher.Collector.App.DI
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

            #region Sources
            services.Configure<SourceTwitterConfiguration>(
                opt => configurationRoot
                .GetSection("sources:twitter")
                .Bind(opt));

            services.AddTransient<SourceTwitterService>();
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