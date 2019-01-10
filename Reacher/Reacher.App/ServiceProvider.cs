using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reacher.Source.Twitter;
using Reacher.Source.Twitter.Configuration;
using System;

namespace Reacher.App
{
    public static class ServiceProvider
    {
        public static IServiceProvider Get(IConfigurationRoot configurationRoot)
        {
            var services = new ServiceCollection();

            services.Configure<SourceTwitterConfiguration>(
                opt => configurationRoot
                .GetSection("sources:twitter")
                .Bind(opt));

            services.AddTransient<SourceTwitterService>();

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}