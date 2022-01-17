using System;
using System.Configuration;

namespace kiranbot.MediaWiki.Configuration
{
    public sealed class ApiSleepSettings : ConfigurationElement
    {
        private static readonly ConfigurationProperty _propName = new ConfigurationProperty("action", typeof(ApiAction), null, null, null, ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired);
        private static readonly ConfigurationProperty _propSleep = new ConfigurationProperty("sleep", typeof(int), null, null, new IntegerValidator(0, int.MaxValue), ConfigurationPropertyOptions.None);
        private static readonly ConfigurationProperty _propShared = new ConfigurationProperty("sharedSleep", typeof(ApiAction), ApiAction.None, null, null, ConfigurationPropertyOptions.None);
        private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();


        // Methods
        static ApiSleepSettings()
        {
            _properties.Add(_propName);
            _properties.Add(_propSleep);
            _properties.Add(_propShared);
        }

        public ApiSleepSettings()
        {
        }

        public ApiSleepSettings(ApiAction action, ApiAction share)
        {
            this.Action = action;
            this.SharedSleep = share;
        }

        public ApiSleepSettings(ApiAction action, int sleep)
        {
            this.Action = action;
            this.Sleep = sleep;
        }

        [ConfigurationProperty("action", Options = ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired)]
        public ApiAction Action
        {
            get
            {
                return (ApiAction)base[_propName];
            }
            set
            {
                base[_propName] = value;
            }
        }

        [ConfigurationProperty("sleep", DefaultValue = 1000)]
        public int Sleep
        {
            get
            {
                return (int)base[_propSleep];
            }
            set
            {
                base[_propSleep] = value;
            }
        }

        [ConfigurationProperty("sharedSleep", DefaultValue = ApiAction.None)]
        public ApiAction SharedSleep
        {
            get
            {
                return (ApiAction)base[_propShared];
            }
            set
            {
                base[_propShared] = value;
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
