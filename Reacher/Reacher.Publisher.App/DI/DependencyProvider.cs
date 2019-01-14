using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Reacher.Destination.Facebook;
using Reacher.Destination.Facebook.Configuration;
using Reacher.Notification.Pushover;
using Reacher.Notification.Pushover.Configuration;
using Reacher.Shared.Common;
using Reacher.Storage.File.Json;
using Reacher.Storage.File.Json.Configuration;
using System;

namespace Reacher.Publisher.App.DI
{
    public static class DependencyProvider
    {
        public static IServiceProvider Get(IConfigurationRoot configurationRoot, string clientId)
        {
            var services = new ServiceCollection();

            #region Client
            services.AddTransient<IClientProvider>(s => new ClientProvider(clientId));
            #endregion

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

            #region Storages
            services.Configure<StorageFileJsonConfiguration>(
                opt => configurationRoot
                .GetSection("storages:file_json")
                .Bind(opt));

            services.AddTransient<IStorageFileJsonService, StorageFileJsonService>();
            #endregion

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}