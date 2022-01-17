using System;
using System.Collections.Generic;
using System.Text;

namespace kiranbot.MediaWiki
{
    [Serializable()]
    public sealed class LoginException : MediaWikiException
    {
        string _result;

        public string Result { get { return _result; } }

        public LoginException(string message, string result)
            : base(message)
        {
            _result = result;
        }
    }
}
