using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eclectica.co.uk.Service.Entities;

namespace eclectica.co.uk.Web.Models
{
    public class IndexViewModel
    {
        public List<EntryModel> Entries { get; set; }

        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
    }
}