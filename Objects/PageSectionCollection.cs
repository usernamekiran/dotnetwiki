using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using kiranbot.ComponentModel;

namespace kiranbot.MediaWiki
{
    [Serializable]
    public sealed class PageSectionCollection : BusinessObjectCollection<PageSectionCollection, PageSection>
    {
        #region Constructors and Factory
        private PageSectionCollection() { }

        private PageSectionCollection(IList<PageSection> list) : base(list) { }

        public static PageSectionCollection NewPageSectionCollection()
        {
            return new PageSectionCollection();
        }

        public static PageSectionCollection GetPageSectionCollection(string content)
        {
            return new PageSectionCollection(ParseSections(content));
        }
        #endregion

        public PageSection[] GetTopLevel()
        {
            List<PageSection> list = new List<PageSection>();

            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].HeadingLevel == PageSection.MinHeadingLevel)
                    list.Add(this[i]);
            }

            return list.ToArray();
        }

        public PageSection[] GetChildren(int index)
        {
            int parentLevel = this[index].HeadingLevel;
            List<PageSection> children = new List<PageSection>();

            for (int i = index + 1; i < this.Count; i++)
            {
                if (this[i].HeadingLevel <= parentLevel) break;

                if (this[i].HeadingLevel == parentLevel + 1)
                    children.Add(this[i]);
            }

            return children.ToArray();
        }

        public PageSection Find(string path)
        {
            string[] parts = path.Split('/');
            return FindInternal(parts, 0, PageSection.MinHeadingLevel, 0);
        }

        private PageSection FindInternal(string[] findPath, int pathIndex, int level, int startIndex)
        {
            for (int i = startIndex; i < this.Count; i++)
            {
                if (this[i].HeadingLevel < level) return null;

                if (this[i].HeadingLevel == level && this[i].Heading == findPath[pathIndex])
                {
                    if (findPath.Length == pathIndex + 1) 
                        return this[i];
                    else
                    {
                        PageSection found = FindInternal(findPath, pathIndex + 1, level + 1, i + 1);
                        if (found != null) return found;
                    }
                }
            }

            return null;
        }

        public PageSection Add(string content)
        {
            PageSection newSection = PageSection.NewPageSection();
            newSection.Content = content;
            Add(newSection);
            return newSection;
        }

        public PageSection Add(string heading, int headingLevel, string content)
        {
            PageSection newSection = PageSection.NewPageSection();
            newSection.Heading = heading;
            newSection.HeadingLevel = headingLevel;
            newSection.Content = content;
            Add(newSection);
            return newSection;
        }

        internal void RenderContent(StringBuilder sb)
        {
            if (this.Count == 0) return;

            foreach (PageSection section in this)
            {
                section.RenderContent(sb);
            }
        }

        #region Section parsing
        private static IList<PageSection> ParseSections(string content)
        {
            if (content == null) throw new ArgumentNullException("content");

            List<PageSection> sections = new List<PageSection>();

            if (content.Length == 0) return sections;

            using (StringReader sr = new StringReader(content))
            {
                string sectionHeader = null;
                string sectionContent = null;
                int headingLevel = 1;

                string line = sr.ReadLine();

                while (line != null)
                {
                    if (line.StartsWith("="))
                    {
                        int testLevel = CountChar(line, '=', true) - 1;
                        string testHead = CleanHeader(line, testLevel);

                        if (testHead != null)
                        {
                            //end last section, start new section
                            if (sectionContent != null || sectionHeader != null)
                            {
                                //add section and reset
                                sections.Add(PageSection.GetPageSection(sectionHeader, headingLevel, sectionContent));
                                sectionHeader = null;
                                sectionContent = null;
                                headingLevel = 1;
                            }

                            headingLevel = testLevel;
                            sectionHeader = testHead;

                            if (headingLevel == 0)
                            {
                                //bad heading, auto clean to level 1:
                                headingLevel = 1;
                            }

                            line = null;
                        }
                    }

                    if (line != null)
                    {
                        if (sectionContent == null)
                            sectionContent = string.Empty;
                        else if (sectionContent.Length > 0)
                            sectionContent += "\n";

                        sectionContent += line;
                    }

                    line = sr.ReadLine();
                }

                if (sectionContent != null || sectionHeader != null)
                {
                    //add section and reset
                    sections.Add(PageSection.GetPageSection(sectionHeader, headingLevel, sectionContent));
                    sectionHeader = null;
                    sectionContent = null;
                    headingLevel = 1;
                }
            }

            return sections;
        }

        private static string CleanHeader(string header, int level)
        {
            header = header.Substring(level + 1);
            header = header.Trim();

            if (!header.EndsWith(new string('=', level + 1))) return null;

            header = header.Substring(0, header.Length - (level + 1));

            return header.Trim();
        }

        private static int CountChar(string toSearch, char toCount, bool consecutive)
        {
            int total = 0;
            for (int i = 0; i < toSearch.Length; i++)
            {
                if (consecutive && toSearch[i] != toCount) return total;

                if (toSearch[i] == toCount) total++;
            }

            return total;
        }
        #endregion
    }

}
