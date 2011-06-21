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


        private static Regex _tags = new Regex("<[^>]*(>|$)", RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Compiled);
        private static Regex _whitelist = new Regex(@"
                                                    ^</?(a|b|em|i|li|ol|p|s(trong|trike)?|ul)>$
                                                    |^<br\s?/?>$
                                                    |^<a[^>]+>$",
                                                    RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace |
                                                    RegexOptions.ExplicitCapture | RegexOptions.Compiled);

        /// <summary>
        /// sanitize any potentially dangerous tags from the provided raw HTML input using 
        /// a whitelist based approach, leaving the "safe" HTML tags
        /// </summary>
        public static string Sanitize(this string s)
        {

            string tagname = "";
            Match tag;
            MatchCollection tags = _tags.Matches(s);

            // iterate through all HTML tags in the input
            for (int i = tags.Count - 1; i > -1; i--)
            {
                tag = tags[i];
                tagname = tag.Value.ToLower();

                if (!_whitelist.IsMatch(tagname))
                {
                    // not on our whitelist? I SAY GOOD DAY TO YOU, SIR. GOOD DAY!
                    s = s.Remove(tag.Index, tag.Length);
                }
                else if (tagname.StartsWith("<a"))
                {
                    // detailed <a> tag checking
                    if (!IsMatch(tagname,
                        @"<a\s
                  href=""(\#\d+|(https?|ftp)://[-A-Za-z0-9+&@#/%?=~_|!:,.;]+)""
                  (\stitle=""[^""]+"")?\s?>"))
                    {
                        s = s.Remove(tag.Index, tag.Length);
                    }
                }
                else if (tagname.StartsWith("<img"))
                {
                    // detailed <img> tag checking
                    if (!IsMatch(tagname,
                        @"<img\s
              src=""https?://[-A-Za-z0-9+&@#/%?=~_|!:,.;]+""
              (\swidth=""\d{1,3}"")?
              (\sheight=""\d{1,3}"")?
              (\salt=""[^""]*"")?
              (\stitle=""[^""]*"")?
              \s?/?>"))
                    {
                        s = s.Remove(tag.Index, tag.Length);
                    }
                }

            }

            return s;
        }


        /// <summary>
        /// Utility function to match a regex pattern: case, whitespace, and line insensitive
        /// </summary>
        private static bool IsMatch(string s, string pattern)
        {
            return Regex.IsMatch(s, pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase |
                RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture);
        }
    }
}
