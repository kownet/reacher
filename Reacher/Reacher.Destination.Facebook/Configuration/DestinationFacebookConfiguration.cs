namespace Reacher.Destination.Facebook.Configuration
{
    public class DestinationFacebookConfiguration
    {
        public string FanPage { get; set; }
        public ConfigurationKeys AccessKeys { get; set; }

        public class ConfigurationKeys
        {
            public string AccessKey { get; set; }
        }
    }
}