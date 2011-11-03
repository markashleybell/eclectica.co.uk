using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace eclectica.co.uk.Web.Extensions
{
    public static class Collection
    {
        public static bool HasNoItems<T>(this IEnumerable<T> c)
        {
            return (c == null || c.Count() == 0);
        }

        public static bool HasNoItems<T>(this ICollection<T> c)
        {
            return (c == null || c.Count() == 0);
        }

        public static bool HasNoItems<T>(this IList<T> c)
        {
            return (c == null || c.Count() == 0);
        }

        public static bool HasNoItems<T>(this IQueryable<T> c)
        {
            return (c == null || c.Count() == 0);
        }

        public static string[] SplitOrNull(this string s, string splitOn)
        {
            if(s == null) return null;
            return s.Split(new string[] { splitOn }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}