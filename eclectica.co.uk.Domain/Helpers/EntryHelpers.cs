using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace eclectica.co.uk.Domain.Helpers
{
    public static class EntryHelpers
    {
        public static string GetThumbnail(string title, string body)
        {
            return GetThumbnail(title, body, false);
        }

        public static string GetThumbnail(string title, string body, bool searchResults)
        {
            if (body == null)
                return "";

            MatchCollection matches = Regex.Matches(body, @"\/img/lib/(.*?)/(.*?)\.(jpg|gif)", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            string thumb = (matches.Count > 0) ? matches[0].Groups[2].Value : "";
            bool drop = (matches.Count > 0 && matches[0].Groups[1].Value == "drop") ? true : false;
            string ext = (matches.Count > 0) ? matches[0].Groups[3].Value : "";

            // Drop images won't have a thumbnail, so we'll have to show the default article image in search results
            string bg = (matches.Count > 0 && !(searchResults && drop)) ? "lib/" + ((drop) ? "drop" : "crop") + "/" + thumb + "." + ext : "";

            if (bg == "" && body.Contains("<object"))
                bg = "site/thumb-video.gif";

            if (bg == "")
                bg = "site/" + ((title == "") ? "thumb-quote" : "thumb-article") + ".gif";

            return bg;
        }
    }
}
