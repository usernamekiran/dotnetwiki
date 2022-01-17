using System;
using System.Collections.Generic;
using System.Text;

namespace kiranbot.MediaWiki
{
    public static class ContentHelper
    {
        public const string UnorderedListPrefix = "*";
        public const string OrderedListPrefix = "#";

        public static string ToUnorderedList(params string[] items)
        {
            if (items == null) throw new ArgumentNullException("items");
            if (items.Length == 0) return string.Empty;

            return UnorderedListPrefix + string.Join("\n" + UnorderedListPrefix, items);
        }

        public static string ToOrderedList(params string[] items)
        {
            if (items == null) throw new ArgumentNullException("items");
            if (items.Length == 0) return string.Empty;

            return OrderedListPrefix + string.Join("\n" + OrderedListPrefix, items);
        }

        public const string DateFormat = "[[yyyy-MM-dd]]";

        public static string GetLink(IPage page)
        {
            if (page == null) throw new ArgumentNullException("page");

            return string.Format("[[{0}]]", NamespaceUtility.GetFullTitle(page));
        }

        public static string GetLink(MediaWikiNamespace ns, string title, string display)
        {
            if (string.IsNullOrEmpty(title)) throw new ArgumentNullException("title");
            if (string.IsNullOrEmpty(display)) throw new ArgumentNullException("display");

            return string.Format("[[{0}|{1}]]", NamespaceUtility.GetFullTitle(ns, title), display);
        }

        public static string GetLink(MediaWikiNamespace ns, string title)
        {
            if (string.IsNullOrEmpty(title)) throw new ArgumentNullException("title");

            return string.Format("[[{0}]]", NamespaceUtility.GetFullTitle(ns, title));
        }

        public static string GetLink(IPage page, string display)
        {
            if (string.IsNullOrEmpty(display)) throw new ArgumentNullException("display");
            if (page == null) throw new ArgumentNullException("page");

            return string.Format("[[{0}|{1}]]", NamespaceUtility.GetFullTitle(page), display);
        }

        public static string PipeTrick(MediaWikiNamespace ns, string title)
        {
            if(string.IsNullOrEmpty(title)) throw new ArgumentNullException(title);

            //any namespace prefix (such as "Help:") or an interwiki prefix (such as "commons:") is removed. This applies to any word before the first colon (:). Therefore only the first prefix is removed, and if a colon precedes the prefix, it will not be removed. 
            //if there is text in parentheses at the end it will be removed 
            //if there are no parentheses but there is a comma, the comma and everything after it is removed 
            title = title.Trim();
            title = NamespaceUtility.StripNamespace(ns, title);

            int paren = title.IndexOf('(');

            if (paren >= 0 && title.EndsWith(")"))
                return title.Substring(0, paren);

            int comma = title.IndexOf(",");

            if (comma >= 0)
                return title.Substring(0, comma);

            return title;
        }
    }
}
