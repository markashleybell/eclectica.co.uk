using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eclectica.co.uk.Web.Extensions
{
    public static class ModelStateExtensions
    {
        public static dynamic GetErrorsForJSON(this ModelStateDictionary dictionary)
        {
            return (from i in dictionary
                    where i.Value.Errors.Count > 0
                    select new {
                        Property = i.Key,
                        Errors = (from e in i.Value.Errors
                                  select e.ErrorMessage).ToArray()
                    }).ToList();
        }
    }
}