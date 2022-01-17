using System;
using System.Configuration;

namespace kiranbot.MediaWiki.Configuration
{
    public sealed class MediaWikiSection : ConfigurationSection
    {
        //TODO: need to validate that query sleep is >= 1000

         // Fields
        private static readonly ConfigurationProperty _propSleep = new ConfigurationProperty("sleep", typeof(ApiSleepSettingsCollection), null, ConfigurationPropertyOptions.None);
        private static readonly ConfigurationProperty _propSiteName = new ConfigurationProperty("siteName", typeof(string), "Wikipedia", null, new StringValidator(1), ConfigurationPropertyOptions.None);
        private static readonly ConfigurationProperty _propServer = new ConfigurationProperty("server", typeof(string), "http://en.wikipedia.org", null, new StringValidator(1), ConfigurationPropertyOptions.None);
        private static readonly ConfigurationProperty _propScriptName = new ConfigurationProperty("scriptName", typeof(string), "index.php", null, new StringValidator(1), ConfigurationPropertyOptions.None);
        private static readonly ConfigurationProperty _propScriptPath = new ConfigurationProperty("scriptPath", typeof(string), "/w", null, new StringValidator(1), ConfigurationPropertyOptions.None);
        private static readonly ConfigurationProperty _propApiName = new ConfigurationProperty("apiName", typeof(string), "api.php", null, new StringValidator(1), ConfigurationPropertyOptions.None);
        private static readonly ConfigurationProperty _propUserAgentFormat = new ConfigurationProperty("userAgent", typeof(string), "{0} (v. {1})", null, new StringValidator(1), ConfigurationPropertyOptions.None);
        private static readonly ConfigurationProperty _propMaximumRedirect = new ConfigurationProperty("maxRedirectFollow", typeof(int), 5, null, new IntegerValidator(-1, 100), ConfigurationPropertyOptions.None);
        private static readonly ConfigurationProperty _propDefaultUser = new ConfigurationProperty("defaultLoginName", typeof(string), null, null, new StringValidator(1), ConfigurationPropertyOptions.None);
        private static readonly ConfigurationProperty _propLogins = new ConfigurationProperty("logins", typeof(ApiLoginSettingsCollection), null, ConfigurationPropertyOptions.None);
                
        private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

        // Methods
        static MediaWikiSection()
        {
            _properties.Add(_propSleep);
            _properties.Add(_propSiteName);
            _properties.Add(_propServer);
            _properties.Add(_propScriptName);
            _properties.Add(_propScriptPath);
            _properties.Add(_propApiName);
            _properties.Add(_propUserAgentFormat);
            _properties.Add(_propMaximumRedirect);
            _properties.Add(_propDefaultUser);
            _properties.Add(_propLogins);
        }

        protected override void InitializeDefault()
        {
            base.InitializeDefault();

            Sleep.Add(new ApiSleepSettings(ApiAction.Query, 5000));
            Sleep.Add(new ApiSleepSettings(ApiAction.Submit, 60000));
            Sleep.Add(new ApiSleepSettings(ApiAction.Login, ApiAction.Submit));
            Sleep.Add(new ApiSleepSettings(ApiAction.FeedWatchlist, ApiAction.Submit));
            Sleep.Add(new ApiSleepSettings(ApiAction.OpenSearch, ApiAction.Query));
            Sleep.Add(new ApiSleepSettings(ApiAction.Help, ApiAction.Query));
        }

        protected override object GetRuntimeObject()
        {
            this.SetReadOnly();
            return this;
        }

        // Properties
        [ConfigurationProperty("userAgent", DefaultValue = "{0} (v. {1})")]
        public string UserAgentFormat
        {
            get
            {
                return (string)base[_propUserAgentFormat];
            }
        }

        [ConfigurationProperty("maxRedirectFollow", DefaultValue = 5)]
        public int MaxRedirectFollow
        {
            get
            {
                return (int)base[_propMaximumRedirect];
            }
        }

        [ConfigurationProperty("defaultLoginName")]
        public string DefaultLoginName
        {
            get
            {
                return (string)base[_propDefaultUser];
            }
            set
            {
                base[_propDefaultUser] = value;
            }
        }

        [ConfigurationProperty("logins")]
        public ApiLoginSettingsCollection Logins
        {
            get
            {
                return (ApiLoginSettingsCollection)base[_propLogins];
            }
        }

        [ConfigurationProperty("sleep")]
        public ApiSleepSettingsCollection Sleep
        {
            get
            {
                return (ApiSleepSettingsCollection)base[_propSleep];
            }
        }

        [ConfigurationProperty("siteName", DefaultValue="Wikipedia")]
        public string SiteName
        {
            get
            {
                return (string)base[_propSiteName];
            }
        }

        [ConfigurationProperty("server", DefaultValue = "http://en.wikipedia.org")]
        public string Server
        {
            get
            {
                return (string)base[_propServer];
            }
        }

        [ConfigurationProperty("scriptName", DefaultValue = "index.php")]
        public string ScriptName
        {
            get
            {
                return (string)base[_propScriptName];
            }
        }

        [ConfigurationProperty("apiName", DefaultValue = "api.php")]
        public string ApiName
        {
            get
            {
                return (string)base[_propApiName];
            }
        }

        [ConfigurationProperty("scriptPath", DefaultValue = "/w")]
        public string ScriptPath
        {
            get
            {
                return (string)base[_propScriptPath];
            }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return _properties;
            }
        }
    }
}
