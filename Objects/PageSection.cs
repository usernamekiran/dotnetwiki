using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using kiranbot.ComponentModel;

namespace kiranbot.MediaWiki
{
   
    [Serializable]
    public sealed class PageSection : BusinessObject<PageSection>
    {
        public const int MinHeadingLevel = 1;
        public const int MaxHeadingLevel = 5;

        int _headingLevel = MinHeadingLevel;
        string _heading;
        string _content = String.Empty;

        #region Constructors and Factory
        private PageSection()
        {
        }

        private PageSection(string content)
        {
            _content = content;
        }
        private PageSection(string heading, int headingLevel, string content)
        {
            if (headingLevel < MinHeadingLevel || headingLevel > MaxHeadingLevel) throw new ArgumentOutOfRangeException("headingLevel");

            _heading = heading;
            _headingLevel = headingLevel;
            _content = content;
        }

        public static PageSection NewPageSection()
        {
            PageSection o = new PageSection();
            o.MarkDirty();
            return o;
        }

        public static PageSection GetPageSection(string content)
        {
            PageSection o = new PageSection(content);
            o.MarkClean();
            return o;
        }

        public static PageSection GetPageSection(string heading, int headingLevel, string content)
        {
            PageSection o = new PageSection(heading, headingLevel, content);
            o.MarkClean();
            return o;
        }
        #endregion

        public void AppendContent(string newContent)
        {
            if (string.IsNullOrEmpty(_content)) 
                Content = newContent;
            else
                Content += newContent;
        }

        public string Heading 
        { 
            get 
            { 
                return _heading; 
            }
            set
            {
                CanWriteProperty();
                _heading = value;
                MarkDirty();
            }
        }
        public string Content 
        { 
            get 
            { return _content; }
            set
            {
                CanWriteProperty();
                _content = value;
                MarkDirty();
            }
        }
        public int HeadingLevel
        {
            get
            {
                return _headingLevel;
            }
            set
            {
                CanWriteProperty();
                if (value < MinHeadingLevel || value > MaxHeadingLevel) throw new ArgumentOutOfRangeException("value");
                _headingLevel = value;
                MarkDirty();
            }
        }

        internal void RenderContent(StringBuilder sb)
        {
            sb.AppendLine();

            if (string.IsNullOrEmpty(Heading))
            {
                sb.Append(Content);
                return;
            }

            sb.AppendFormat("{0} {1} {0}\n{2}", new string('=', HeadingLevel + 1), Heading, Content);
        }
    }
}
