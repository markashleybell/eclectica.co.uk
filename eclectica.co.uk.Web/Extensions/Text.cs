using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eclectica.co.uk.Web.Extensions
{
    public static class Text
    {
        public static string Capitalise(this string s)
        {
            return s[0].ToString().ToUpper() + s.Substring(1);
        }
    }
}