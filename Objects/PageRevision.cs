using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using kiranbot.ComponentModel;

namespace kiranbot.MediaWiki
{
    [Serializable]
    public class PageRevision : BusinessObject<PageRevision>
    {
        DateTime? _timestamp;
        bool _isMinor;
        string _user;
        string _comment;
        PageSectionCollection _sections = PageSectionCollection.NewPageSectionCollection();
        string _redirectTitle;
        CategoryCollection _explicitCategories = CategoryCollection.NewCategoryCollection();

        #region Constructors and Factory
        public static PageRevision NewPageRevision()
        {
            return new PageRevision();
        }

        internal static PageRevision GetPageRevision(XmlElement revision, bool isRedirect)
        {
            PageRevision pr = new PageRevision(revision, isRedirect);
            pr.MarkClean();
            pr.SetReadOnly();
            return pr;
        }

        private PageRevision() { }

        private PageRevision(XmlElement revision, bool isRedirect)
        {
            string text = revision.InnerText;

            
            if (isRedirect)
            {
                Match redirectMatch = Regex.Match(text, MediaWikiApi.RedirectRegex);
                _redirectTitle = redirectMatch.Groups["title"].Value;
                text = text.Replace(redirectMatch.Value, string.Empty);
            }

            MatchCollection categoryMatches = Regex.Matches(text, MediaWikiApi.CategoryRegex);
            foreach (Match m in categoryMatches)
            {
                Category newCategory = _explicitCategories.Add(m.Groups["title"].Value);
                text = text.Replace(m.Value, string.Empty);
            }

            _sections = PageSectionCollection.GetPageSectionCollection(text);
            _user = revision.Attributes["user"].Value;
            _timestamp = DateTime.Parse(revision.Attributes["timestamp"].Value);

            XmlAttribute commentAtt = revision.Attributes["comment"];

            if (commentAtt != null)
                _comment = revision.Attributes["comment"].Value;
        }
        #endregion

        public override void SetReadOnly()
        {
            base.SetReadOnly();
            _sections.SetReadOnly();
            _explicitCategories.SetReadOnly();
        }

        public override void SetEditable()
        {
            base.SetEditable();
            _sections.SetEditable();
            _explicitCategories.SetEditable();
        }

        public override bool IsDirty
        {
            get
            {
                return base.IsDirty || _sections.IsDirty || _explicitCategories.IsDirty;
            }
        }

        public DateTime? Timestamp { get { return _timestamp; } }
        public PageSectionCollection Sections { get { return _sections; } }

        public bool IsRedirect { get { return !string.IsNullOrEmpty(_redirectTitle); } }

        public string RedirectTitle 
        { 
            get 
            { 
                return _redirectTitle; 
            }
            set
            {
                CanWriteProperty();
                _redirectTitle = value;
                MarkDirty();
            }
        }

        public string Comment
        {
            get
            {
                return _comment;
            }
            set
            {
                CanWriteProperty();
                _comment = value;
                MarkDirty();
            }
        }

        public bool IsMinor
        {
           get
            {
                return _isMinor;
            }
            set
            {
                CanWriteProperty();
                _isMinor = value;
                MarkDirty();
            }
        }

        public CategoryCollection Categories 
        { 
            get 
            { 
                return _explicitCategories; 
            } 
        }

        public void AppendContent(string newContent)
        {
            if (_sections.Count == 0)
                _sections.Add(newContent);
            else
                _sections[_sections.Count - 1].AppendContent(newContent);
        }

        public string GetContent()
        {
            if (!string.IsNullOrEmpty(_redirectTitle)) return string.Format("#REDIRECT [[{0}]]", _redirectTitle);

            StringBuilder sb = new StringBuilder();

            _sections.RenderContent(sb);
            sb.AppendLine();
            _explicitCategories.RenderContent(sb);

            return sb.ToString();
        }
    }
}
