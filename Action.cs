using System;
using System.Collections.Generic;
using System.Text;

namespace kiranbot.MediaWiki
{
    public enum ApiAction
    {
        None = 0,
        Query,
        Submit,
        Login,
        OpenSearch,
        FeedWatchlist,
        Help,
        Move
    };

    internal static class ActionUtility
    {
        public static string ActionToString(ApiAction action)
        {
            switch(action)
            {
                case ApiAction.Query:
                    return "query";
                case ApiAction.Submit:
                    return "submit";
                case ApiAction.Login:
                    return "login";
                case ApiAction.OpenSearch:
                    return "opensearch";
                case ApiAction.FeedWatchlist:
                    return "feedwatchlist";
                case ApiAction.Help:
                    return "help";
                case ApiAction.Move:
                    return "move";
            }

            throw new ArgumentOutOfRangeException("action");
        }
    }
}
