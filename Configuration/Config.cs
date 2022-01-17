using System;
using System.Configuration;
using kiranbot.MediaWiki.Configuration;

namespace kiranbot.MediaWiki
{
    internal static class Config
    {
        public static Configuration.ApiSleepSettingsCollection Sleep
        {
            get
            {
                return Section.Sleep;
            }
        }

        public static Configuration.ApiLoginSettingsCollection Logins
        {
            get
            {
                return Section.Logins;
            }
        }

        public static string DefaultLoginName
        {
            get
            {
                return Section.DefaultLoginName;
            }
        }

        private static MediaWikiSection Section
        {
            get
            {
                return (Configuration.MediaWikiSection)ConfigurationManager.GetSection("mediaWiki");                
            }
        }

        public static string SiteName
        {
            get
            {
                return Section.SiteName;
            }
        }

        public static string Server
        {
            get
            {
                return Section.Server;
            }
        }

        public static string ScriptName
        {
            get
            {
                return Section.ScriptName;
            }
        }

        public static string ScriptPath
        {
            get
            {
                return Section.ScriptPath;
            }
        }

        public static string ApiName
        {
            get
            {
                return Section.ApiName;
            }
        }

        public static string UserAgentFormat
        {
            get
            {
                return Section.UserAgentFormat;
            }
        }

        public static int MaxRedirectFollow
        {
            get
            {
                return Section.MaxRedirectFollow;
            }
        }
    }
}