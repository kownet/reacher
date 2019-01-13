using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Reacher.Destination.Facebook;
using Reacher.Destination.Facebook.Configuration;
using System;

namespace Reacher.Publisher.App.DI
{
    public static class DependencyProvider
    {
        public static IServiceProvider Get(IConfigurationRoot configurationRoot)
        {
            var services = new ServiceCollection();

            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Trace);
                builder.AddNLog(new NLogProviderOptions
                {
                    CaptureMessageTemplates = true,
                    CaptureMessageProperties = true
                });
            });

            services.Configure<DestinationFacebookConfiguration>(
                opt => configurationRoot
                .GetSection("destinations:facebook")
                .Bind(opt));

            services.AddTransient<DestinationFacebookService>();

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}