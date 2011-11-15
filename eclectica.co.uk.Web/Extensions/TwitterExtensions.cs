using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eclectica.co.uk.Web.Extensions
{
    public static class TwitterExtensions
    {
        public static string ToPrettyDate(this string s)
        {
            // Wed Jun 16 12:28:03 +0000 2010
            var twitterDate = DateTime.ParseExact(s, "ddd MMM dd HH:mm:ss %zzzz yyyy", null);

            var diff = (DateTime.Now - twitterDate).TotalMilliseconds / 1000;

            var day_diff = Math.Floor(diff / 86400D);

            if (day_diff < 0 || day_diff >= 31) return twitterDate.ToString("dd/MM/yyyy hh:mm");

            if (day_diff == 0)
            {
                if (diff < 60) { return "just now"; }
                else if (diff < 120) { return "1 minute ago"; }
                else if (diff < 3600) { return Math.Floor(diff / 60D) + " minutes ago"; }
                else if (diff < 7200) { return "1 hour ago"; }
                else if (diff < 86400) { return Math.Floor((diff / 60D) / 60D) + " hours ago"; }
            }
            else
            {
                if (day_diff == 1) { return "Yesterday"; }
                else if (day_diff < 7) { return day_diff + " days ago"; }
                else if (day_diff < 14) { return Math.Floor(day_diff / 7D) + " week ago"; }
                else if (day_diff > 7) { return Math.Ceiling(day_diff / 7D) + " weeks ago"; }
            }

            return s;
        }
    }
}