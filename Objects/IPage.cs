using System;
using System.Collections.Generic;
using System.Text;

namespace kiranbot.MediaWiki
{
    public interface IPage
    {
        string Title { get; }
        MediaWikiNamespace Namespace { get; }
    }
}
