using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace eclectica.co.uk.Web.Extensions
{
    public static class Text
    {
        public static string Capitalise(this string s)
        {
            return s[0].ToString().ToUpper() + s.Substring(1);
        }

        public static string StripHtml(this string s)
        {
            return Regex.Replace(s, @"<(.|\n)+?>", @"");
        }

        public static string EscapeSingleQuotes(this string s)
        {
            return Regex.Replace(s, "'", "\\'");
        }

        public static string Truncate(this string s, int length)
        {
            if (s == null)
                return null;
            return (s.Length > length) ? s.Substring(0, length - 3) + "..." : s;
        }

        public static string FormatComment(this string s)
        {
            RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Singleline;
            Regex _tags = new Regex("<[^>]*(>|$)", RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Compiled);
            // If there's no HTML in here, assume we're dealing with a plain text comment
            if (_tags.Matches(s).Count == 0)
            {
                // Replace double line breaks with <p> tags and single breaks with <br />
                s = Regex.Replace(s, @"(\r\n){2}", "</p><p>", options);
                s = Regex.Replace(s, @"(\r\n){1}", "<br />", options);
            }

            s = (!s.StartsWith("<p>") && !s.EndsWith("</p>")) ? "<p>" + s + "</p>" : s;

            s = Regex.Replace(s, "<p><p>", "<p>", options);
            s = Regex.Replace(s, "</p></p>", "</p>", options);

            return s;
        }
    }
}