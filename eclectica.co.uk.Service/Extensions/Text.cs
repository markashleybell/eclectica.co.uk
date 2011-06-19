using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace eclectica.co.uk.Service.Extensions
{
    public static class Text
    {
        public static string StripHtml(this string s)
        {
            return Regex.Replace(s, @"<(.|\n)+?>", @"");
        }

        public static string GetRelatedThumbnail(this string s, string title)
        {
            MatchCollection matches = Regex.Matches(s, @"\/img/lib/(.*?)/(.*?)\.(jpg|gif)", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            string thumb = (matches.Count > 0) ? matches[0].Groups[2].Value : "";
            bool drop = (matches.Count > 0 && matches[0].Groups[1].Value == "drop") ? true : false;
            string ext = (matches.Count > 0) ? matches[0].Groups[3].Value : "";
            string bg = (matches.Count > 0) ? "lib/" + ((drop) ? "drop" : "crop") + "/" + thumb + "." + ext : "";

            if (bg == "" && s.Contains("<object"))
                bg = "site/thumb-video.gif";

            if (bg == "")
                bg = "site/" + ((title == "") ? "thumb-quote" : "thumb-article") + ".gif";

            return bg;
        }
    }
}
