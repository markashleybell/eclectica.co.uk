using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eclectica.co.uk.Service.Entities;
using System.Web.Mvc;

namespace eclectica.co.uk.Web.Models
{
    public class RedirectEditViewModel
    {
        public RedirectModel Redirect { get; set; }

        public IQueryable<SelectListItem> Redirects { get; set; }
    }
}