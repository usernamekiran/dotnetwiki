using System;
using System.Xml;
using kiranbot.ComponentModel;

namespace kiranbot.MediaWiki
{
    //TODO: merge with category?
    [Serializable]
    public sealed class Link : BusinessObject<Link>, IPage
    {
        string _title;
        MediaWikiNamespace _ns;

        private Link() { }

        public static Link NewLink(MediaWikiNamespace ns, string title)
        {
            Link o = new Link();
            o._title = title;
            o._ns = ns;
            o.MarkDirty();
            return o;
        }

        public static Link GetLink(MediaWikiNamespace ns, string title)
        {
            Link o = new Link();
            o._title = title;
            o._ns = ns;
            o.MarkClean();
            return o;
        }

        internal static Link GetLink(XmlElement link)
        {
            //TODO: this is redundant with page class, centralize somewhere...
            MediaWikiNamespace ns = MediaWikiNamespace.Main;
            XmlAttribute nsAtt = link.Attributes["ns"];
            if (nsAtt != null) ns = (MediaWikiNamespace)Enum.ToObject(typeof(MediaWikiNamespace), int.Parse(nsAtt.Value));

            string title = NamespaceUtility.StripNamespace(ns, link.Attributes["title"].Value);

            return GetLink(ns, title);
        }

        public Page GetPage()
        {
            return Page.GetPage(_ns, _title);
        }

        #region IPage Members

        public string Title
        {
            get { return _title; }
        }

        public MediaWikiNamespace Namespace
        {
            get { return _ns; }
        }

        #endregion
    }
}
