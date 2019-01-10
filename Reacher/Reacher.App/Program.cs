using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reacher.Source.Twitter;
using System;
using System.IO;

namespace Reacher.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            
            var configuration = builder.Build();

            var servicesProvider = ServiceProvider.Get(configuration);

            servicesProvider.GetRequiredService<SourceTwitterService>().Monitor();

            Console.ReadLine();
        }
    }
}