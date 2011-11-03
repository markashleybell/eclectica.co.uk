using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace eclectica.co.uk.Domain.Extensions
{
    public static class Text
    {
        public static string ConvertToSafeUrl(this string s)
        {
            RegexOptions o = RegexOptions.IgnoreCase | RegexOptions.Singleline;

            // Remove all special chars (but not spaces or dashes)
            string output = Regex.Replace(s, @"[^a-z0-9\s\-]", "", o);

            // Replace spaces with hyphens
            output = Regex.Replace(output, @"[\s]", "-", o);

            // Replace multiple hyphens (more than one in a row) with a single hyphen
            output = Regex.Replace(output, @"\-{2,}", "-", o);

            // Trim the extra hyphen off the end if exists
            if (output.Length > 0 && output[output.Length - 1] == '-')
                output = output.Substring(0, output.Length - 1);

            return output.ToLower();
        }
    }
}
