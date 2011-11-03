using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eclectica.co.uk.Service.Entities;
using System.Web.Mvc;

namespace eclectica.co.uk.Web.Models
{
    public class CommentEditViewModel
    {
        public CommentModel Comment { get; set; }

        public IQueryable<SelectListItem> Comments { get; set; }
    }
}