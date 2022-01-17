using System;
using System.Net;

namespace kiranbot.MediaWiki
{
    [Serializable()]
    public class EditException : MediaWikiException
    {
        HttpWebResponse _response;
        
        public EditException(HttpWebResponse response)
            : base("An error occurred while editing this page")
        {
            _response = response;
        }
    }
}
