using System;
using System.Collections.Generic;
using System.Configuration;

namespace kiranbot.MediaWiki.Configuration
{
    [ConfigurationCollection(typeof(ApiSleepSettings))]
    public sealed class ApiSleepSettingsCollection : ConfigurationElementCollection
    {
        // Fields
        private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

        // Methods
        public ApiSleepSettingsCollection()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        public void Add(ApiSleepSettings settings)
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
            return new ApiSleepSettings();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ApiSleepSettings)element).Action;
        }

        public void Remove(ApiSleepSettings settings)
        {
            if (base.BaseIndexOf(settings) >= 0)
            {
                base.BaseRemove(settings.Action);
            }
        }

        public void Remove(ApiAction action)
        {
            base.BaseRemove(action);
        }

        public void RemoveAt(int index)
        {
            base.BaseRemoveAt(index);
        }

        public ApiSleepSettings this[int index]
        {
            get
            {
                return (ApiSleepSettings)base.BaseGet(index);
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

        public bool ContainsKey(ApiAction action)
        {
            return this.BaseGet(action) != null;
        }

        public ApiSleepSettings GetSleep(ApiAction action)
        {
            return (ApiSleepSettings)this.BaseGet(action);
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
