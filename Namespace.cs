using System;
using System.Collections.Generic;
using System.Text;

namespace kiranbot.MediaWiki
{
    internal static class NamespaceUtility
    {
        internal static string StripNamespace(MediaWikiNamespace ns, string title)
        {
            if (ns == MediaWikiNamespace.Main) return title;

            string nsPrefix = NamespaceToPrefix(ns);
            if (title.StartsWith(nsPrefix)) return title.Substring(nsPrefix.Length);

            return title;
        }

        internal static string GetFullTitle(IPage page)
        {
            if (page == null) throw new ArgumentNullException("page");

            return GetFullTitle(page.Namespace, page.Title);
        }

        internal static string GetFullTitle(MediaWikiNamespace ns, string title)
        {
            return NamespaceToPrefix(ns) + title;
        }

        internal static string NamespaceToPrefix(MediaWikiNamespace ns)
        {
            switch (ns)
            {
                case MediaWikiNamespace.Category:
                    return "Category:";
                case MediaWikiNamespace.CategoryTalk:
                    return "Category talk:";
                case MediaWikiNamespace.Help:
                    return "Help:";
                case MediaWikiNamespace.HelpTalk:
                    return "Help talk:";
                case MediaWikiNamespace.Image:
                    return "Image:";
                case MediaWikiNamespace.ImageTalk:
                    return "Image talk:";
                case MediaWikiNamespace.Main:
                    return "";
                case MediaWikiNamespace.MainTalk:
                    return "Talk:";
                case MediaWikiNamespace.Media:
                    return "Media:";
                case MediaWikiNamespace.Project:
                    return Config.SiteName + ":";
                case MediaWikiNamespace.ProjectTalk:
                    return Config.SiteName + " talk:";
                case MediaWikiNamespace.Special:
                    return "Special:";
                case MediaWikiNamespace.Template:
                    return "Template:";
                case MediaWikiNamespace.TemplateTalk:
                    return "Template talk:";
                case MediaWikiNamespace.User:
                    return "User:";
                case MediaWikiNamespace.UserTalk:
                    return "User talk:";                
            }

            throw new ArgumentOutOfRangeException("ns");
        }
    }

    public enum MediaWikiNamespace
    {
        Media = -2,
        Special = -1,
        Main = 0, //Has no prefix 
        MainTalk = 1,
        User = 2,
        UserTalk = 3,
        Project = 4,
        ProjectTalk = 5,
        Image = 6,
        ImageTalk = 7,
        UICustomization = 8,
        UICustomizationTalk = 9,
        Template = 10,
        TemplateTalk = 11,
        Help = 12,
        HelpTalk = 13,
        Category = 14,
        CategoryTalk = 15
    };
}
