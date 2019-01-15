using System.Collections.Generic;

namespace Reacher.Source.Twitter.Configuration
{
    public class SourceTwitterConfiguration
    {
        public List<string> Hashtags { get; set; }
        public List<string> Accounts { get; set; }
        public ConfigurationKeys AccessKeys { get; set; }

        public class ConfigurationKeys
        {
            public string ConsumerKey { get; set; }
            public string ConsumerSecret { get; set; }
            public string UserAccessToken { get; set; }
            public string UserAccessSecret { get; set; }
        }
    }
}