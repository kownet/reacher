using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reacher.Source.Twitter.Configuration;

namespace Reacher.Source.Twitter
{
    public class SourceTwitterService
    {
        private readonly IOptions<SourceTwitterConfiguration> _configuration;
        private readonly ILogger<SourceTwitterService> _logger;

        public SourceTwitterService(
            IOptions<SourceTwitterConfiguration> configuration,
            ILogger<SourceTwitterService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public void Collect()
        {
            _logger.LogInformation($"Hashtags to search: {string.Join(",", _configuration.Value.Hashtags)}");
        }
    }
}