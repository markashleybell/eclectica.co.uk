using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eclectica.co.uk.Service.Entities;

namespace eclectica.co.uk.Web.Models
{
    public class EntryViewModel
    {
        public EntryModel Entry { get; set; }
        public CommentViewModel Comment { get; set; }
    }
}