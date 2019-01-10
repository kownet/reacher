using Microsoft.Extensions.Options;
using Reacher.Source.Twitter.Configuration;
using System;

namespace Reacher.Source.Twitter
{
    public class SourceTwitterService
    {
        private readonly IOptions<SourceTwitterConfiguration> _configuration;

        public SourceTwitterService(IOptions<SourceTwitterConfiguration> configuration)
        {
            _configuration = configuration;
        }

        public void Monitor()
        {
            Console.WriteLine(string.Join(",", _configuration.Value.Hashtags));
        }
    }
}