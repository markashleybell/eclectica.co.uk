using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eclectica.co.uk.Service.Entities;
using System.Web.Mvc;

namespace eclectica.co.uk.Web.Models
{
    public class EntryEditViewModel
    {
        public EntryModel Entry { get; set; }
        public IEnumerable<ImageModel> Images { get; set; }
        public string Tags { get; set; }

        public IQueryable<SelectListItem> Entries { get; set; }
    }
}