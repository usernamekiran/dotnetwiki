using System;
using System.Configuration;

namespace kiranbot.MediaWiki.Configuration
{
    public sealed class ApiLoginSettings : ConfigurationElement
    {
        private static readonly ConfigurationProperty _propLoginName = new ConfigurationProperty("loginName", typeof(string), null, null, new StringValidator(1), ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired);
        private static readonly ConfigurationProperty _propPassword = new ConfigurationProperty("password", typeof(string), null, null, new StringValidator(1), ConfigurationPropertyOptions.None);
        private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();


        // Methods
        static ApiLoginSettings()
        {
            _properties.Add(_propLoginName);
            _properties.Add(_propPassword);
        }

        public ApiLoginSettings()
        {
        }

        public ApiLoginSettings(string login, string password)
        {
            this.LoginName = login;
            this.Password = password;
        }

        internal string Key
        {
            get
            {
                return this.LoginName;
            }
        }

        [ConfigurationProperty("loginName", Options = ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired, DefaultValue = "")]
        public string LoginName
        {
            get
            {
                return (string)base[_propLoginName];
            }
            set
            {
                base[_propLoginName] = value;
            }
        }

        [ConfigurationProperty("password", Options = ConfigurationPropertyOptions.None)]
        public string Password
        {
            get
            {
                return (string)base[_propPassword];
            }
            set
            {
                base[_propPassword] = value;
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
