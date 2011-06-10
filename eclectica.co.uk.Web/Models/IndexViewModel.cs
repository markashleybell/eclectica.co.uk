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
    }
}