using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reacher.Destination.Facebook.Configuration;

namespace Reacher.Destination.Facebook
{
    public class DestinationFacebookService
    {
        private readonly IOptions<DestinationFacebookConfiguration> _configuration;
        private readonly ILogger<DestinationFacebookService> _logger;

        public DestinationFacebookService(
            IOptions<DestinationFacebookConfiguration> configuration,
            ILogger<DestinationFacebookService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public void Publish()
        {
            _logger.LogInformation($"Fanpage to publish: {_configuration.Value.FanPage}");
        }
    }
}