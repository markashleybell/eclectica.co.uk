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
    }
}
