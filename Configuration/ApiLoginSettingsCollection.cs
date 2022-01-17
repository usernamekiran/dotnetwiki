using System;
using System.Configuration;

namespace kiranbot.MediaWiki.Configuration
{
    [ConfigurationCollection(typeof(ApiLoginSettings))]
    public sealed class ApiLoginSettingsCollection : ConfigurationElementCollection
    {
        // Fields
        private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

        // Methods
        public ApiLoginSettingsCollection()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        public void Add(ApiLoginSettings settings)
        {
            this.BaseAdd(settings);
        }

        protected override void BaseAdd(int index, ConfigurationElement element)
        {
            if (index == -1)
            {
                base.BaseAdd(element, false);
            }
            else
            {
                base.BaseAdd(index, element);
            }
        }

        public void Clear()
        {
            base.BaseClear();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ApiLoginSettings();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ApiLoginSettings)element).Key;
        }

        public void Remove(ApiLoginSettings settings)
        {
            if (base.BaseIndexOf(settings) >= 0)
            {
                base.BaseRemove(settings.Key);
            }
        }

        public void Remove(string name)
        {
            base.BaseRemove(name);
        }

        public void RemoveAt(int index)
        {
            base.BaseRemoveAt(index);
        }

        public ApiLoginSettings this[int index]
        {
            get
            {
                return (ApiLoginSettings)base.BaseGet(index);
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        public bool ContainsKey(string name)
        {
            return this.BaseGet(name) != null;
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return _properties;
            }
        }

        internal ApiLoginSettings GetLogin(string name)
        {
            return (ApiLoginSettings)this.BaseGet(name);
        }
    }
}
