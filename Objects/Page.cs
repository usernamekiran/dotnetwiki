using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using kiranbot.ComponentModel;

namespace kiranbot.MediaWiki
{
    [Serializable]
    public sealed class Page : BusinessObject<Page>, IPage
    {
        int? _id;
        string _title;
        MediaWikiNamespace _ns;
        DateTime _lastTouched;
        int _lastRevisionId;
        int _counter;
        bool _isNewlyCreated;
        PageRevision _lastRevision;
        PageRevision _newRevision;

        private Page()
        {
        }

        #region Fetch / Factory

        public static Page GetPage(string fullTitle)
        {
            return GetPage(MediaWikiApi.GetDefault(), fullTitle);
        }

        public static Page GetPage(MediaWikiNamespace ns, string title)
        {
            return GetPage(MediaWikiApi.GetDefault(), ns, title);
        }

        public static Page GetPage(int id)
        {
            return GetPage(MediaWikiApi.GetDefault(), id);
        }

        public static Page GetPage(string login, string fullTitle)
        {
            return GetPage(MediaWikiApi.GetMediaWikiApi(login), fullTitle);
        }

        public static Page GetPage(string login, int id)
        {
            return GetPage(MediaWikiApi.GetMediaWikiApi(login), id);
        }

        private static Page GetPage(MediaWikiApi api, string fullTitle)
        {
            if (string.IsNullOrEmpty(fullTitle)) throw new ArgumentNullException("fullTitle");

            return GetPage((XmlElement)FetchPageNodes(api, "titles", fullTitle)[0]);
        }

        private static Page GetPage(MediaWikiApi api, MediaWikiNamespace ns, string title)
        {
            if (string.IsNullOrEmpty(title)) throw new ArgumentNullException("title");

            return GetPage((XmlElement)FetchPageNodes(api, "titles", NamespaceUtility.NamespaceToPrefix(ns) + title)[0]);
        }

        private static Page GetPage(MediaWikiApi api, int id)
        {
            return GetPage((XmlElement)FetchPageNodes(api, "pageids", id.ToString())[0]);
        }

        internal static Page GetPage(XmlElement pageElement)
        {
            Page p = new Page();
            p.ParsePage(pageElement);
            p.MarkClean();
            return p;
        }

        internal static XmlDocument FetchPageXml(MediaWikiApi api, string param, string paramValue)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>(3);
            parameters.Add(param, paramValue);
            parameters.Add("prop", "revisions|info");
            parameters.Add("rvprop", "timestamp|user|comment|content");
            parameters.Add("rvlimit", "1");

            return api.RequestApi(ApiAction.Query, parameters);
        }

        private static XmlNodeList FetchPageNodes(MediaWikiApi api, string param, string paramValue)
        {
            XmlDocument xml = FetchPageXml(api, param, paramValue);
            return xml.SelectNodes("/api/query/pages/page");
        }
        
        private void ParsePage(XmlElement page)
        {
            _id = null;

            if (page.Attributes["missing"] == null)
            {
                _id = int.Parse(page.Attributes["pageid"].Value);
            }

            _ns = MediaWikiNamespace.Main;
            XmlAttribute nsAtt = page.Attributes["ns"];
            if (nsAtt != null) _ns = (MediaWikiNamespace)Enum.ToObject(typeof(MediaWikiNamespace), int.Parse(nsAtt.Value));

            _title = NamespaceUtility.StripNamespace(_ns, page.Attributes["title"].Value);

            if (!IsMissing)
            {
                XmlAttribute touched = page.Attributes["touched"];
                if (touched != null)
                {
                    _lastTouched = DateTime.Parse(touched.Value);
                    _counter = int.Parse(page.Attributes["counter"].Value);
                    _lastRevisionId = int.Parse(page.Attributes["lastrevid"].Value);
                    _isNewlyCreated = (page.Attributes["new"] != null);
                }

                _lastRevision = PageRevision.GetPageRevision((XmlElement) page.SelectSingleNode("revisions/rev"), (page.Attributes["redirect"] != null));
            }
        }
        #endregion

        public LinkCollection WhatLinksHere()
        {
            return WhatLinksHere(MediaWikiApi.GetDefault());
        }

        public LinkCollection WhatLinksHere(string loginName)
        {
            return WhatLinksHere(MediaWikiApi.GetMediaWikiApi(loginName));
        }

        public LinkCollection WhatLinksHere(MediaWikiApi api)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>(3);

            parameters.Add("list", "backlinks");
            parameters.Add("bltitle", NamespaceUtility.GetFullTitle(this));
            parameters.Add("bllimit", "500"); //TODO:I think 5000 is limit for bot?

            XmlDocument xml = api.RequestApi(ApiAction.Query, parameters);

            XmlNodeList backLinks = xml.SelectNodes("/api/query/backlinks/bl");

            return LinkCollection.GetLinkCollection(backLinks);
        }

        public Page FollowRuntimeRedirects(string login)
        {
            return FollowRuntimeRedirects(MediaWikiApi.GetMediaWikiApi(login));
        }

        public Page FollowRuntimeRedirects()
        {
            return FollowRuntimeRedirects(MediaWikiApi.GetDefault());
        }

        private Page FollowRuntimeRedirects(MediaWikiApi api)
        {
            return FollowRuntimeRedirectsInternal(api, new Stack<string>());
        }

        private Page FollowRuntimeRedirectsInternal(MediaWikiApi api, Stack<string> redirects)
        {
            if (redirects.Contains(NamespaceUtility.GetFullTitle(this))) throw new InvalidOperationException("Circular redirect detected.");

            if (Config.MaxRedirectFollow == 0 || !LastRevision.IsRedirect) return this;

            if (Config.MaxRedirectFollow == redirects.Count) throw new InvalidOperationException("Long redirect chain detected.");

            redirects.Push(NamespaceUtility.GetFullTitle(this));
            Page redirectTo = Page.GetPage(api, LastRevision.RedirectTitle);
            
            return redirectTo.FollowRuntimeRedirectsInternal(api, redirects);
        }

        #region Save
        public Page Save()
        {
            return Save(MediaWikiApi.GetDefault());
        }

        public Page Save(string login)
        {
            return Save(MediaWikiApi.GetMediaWikiApi(login));
        }

        bool _saved = false;

        private Page Save(MediaWikiApi api)
        {
            if (_saved) throw new InvalidOperationException("Can only save once, used returned page.");

            if (IsDirty)
            {
                string content = _newRevision.GetContent();

                //TODO: individual section edits...
                XmlDocument xml = api.EditPage(NamespaceUtility.GetFullTitle(this), _newRevision.GetContent(), _newRevision.Comment, _newRevision.IsMinor);
                _saved = true;
                //TODO: need to clean this up a bit, move to a dataportal esque style of loading
                return GetPage((XmlElement) xml.SelectSingleNode("/api/query/pages/page"));
            }

            return this;
        }
        #endregion

        public override bool IsDirty
        {
            get
            {
                return base.IsDirty || _newRevision.IsDirty;
            }
        }

        private void CheckMissing()
        {
            if (IsMissing) throw new NotSupportedException("This action is not supported on a missing page.");
        }

        #region Properties

        public bool IsMissing { get { return _id == null; } }
        public string Title { get { return _title; } }
        public MediaWikiNamespace Namespace { get { return _ns; } }
        public int? Id { get { return _id; } }

        public int Counter
        {
            get
            {
                CheckMissing();
                return _counter;
            }
        }

        public DateTime LastTouched
        {
            get
            {
                CheckMissing();
                return _lastTouched;
            }
        }

        public int LastRevisionId
        {
            get
            {
                CheckMissing();
                return _lastRevisionId;
            }
        }

        public PageRevision LastRevision
        {
            get
            {
                CheckMissing();
                return _lastRevision;
            }
        }

        public PageRevision NewRevision
        {
            get
            {
                if (_newRevision == null)
                {
                    if (!IsMissing)
                    {
                        _newRevision = LastRevision.Clone();
                        _newRevision.SetEditable();
                    }
                    else
                        _newRevision = PageRevision.NewPageRevision();
                }

                return _newRevision;
            }
        }

        public bool IsNewlyCreated
        {
            get
            {
                CheckMissing();
                return _isNewlyCreated;
            }
        }

        #endregion
    }
}
