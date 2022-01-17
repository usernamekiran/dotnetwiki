using System;
using System.Collections.Generic;
using System.Xml;
using kiranbot.ComponentModel;

namespace kiranbot.MediaWiki
{
    [Serializable]
    public sealed class LinkCollection : BusinessObjectCollection<LinkCollection, Link>
    {
        private LinkCollection() { }

        internal static LinkCollection GetLinkCollection(XmlNodeList linkElements)
        {
            LinkCollection c = new LinkCollection();

            foreach (XmlElement link in linkElements)
            {
                c.Add(Link.GetLink(link));
            }

            c.MarkClean();
            return c;
        }
    }
}
