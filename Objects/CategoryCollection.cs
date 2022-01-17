using System;
using System.Collections.Generic;
using System.Text;
using kiranbot.ComponentModel;

namespace kiranbot.MediaWiki
{
    [Serializable]
    public sealed class CategoryCollection : BusinessObjectCollection<CategoryCollection, Category>
    {
        public Category Add(string categoryName)
        {
            Category newCategory = Category.NewCategory();
            newCategory.Title = categoryName;
            Add(newCategory);
            return newCategory;
        }

        public bool Contains(string categoryName)
        {
            categoryName = NamespaceUtility.StripNamespace(MediaWikiNamespace.Category, categoryName);

            foreach (Category c in this)
                if (string.CompareOrdinal(c.Title, categoryName) == 0) return true;

            return false;
        }

        public void AddRange(IEnumerable<string> categories)
        {
            foreach (string s in categories)
                Add(s);
        }

        internal void RenderContent(System.Text.StringBuilder sb)
        {
            foreach (Category c in this)
                c.RenderContent(sb);
        }

        internal static CategoryCollection NewCategoryCollection()
        {
            return new CategoryCollection();
        }

        private CategoryCollection() { }
    }
}
