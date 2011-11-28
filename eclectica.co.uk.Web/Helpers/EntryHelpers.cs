using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace eclectica.co.uk.Web.Helpers
{
    public static class EntryHelpers
    {
        public static string GetCaption(string title, string body)
        {
            var options = RegexOptions.Singleline | RegexOptions.IgnoreCase;

            return ((string.IsNullOrEmpty(title)) ? Regex.Replace(Regex.Matches(body, "<p>(.*?)</p>", options)[0].Groups[1].Value, @"<(.|\n)+?>", @"") : title);
        }
    }
}