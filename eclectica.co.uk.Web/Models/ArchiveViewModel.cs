using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eclectica.co.uk.Service.Entities;

namespace eclectica.co.uk.Web.Models
{
    public class ArchiveViewModel : BaseViewModel
    {
        public DateTime Date { get; set; }
        public IDictionary<DateTime, int> Months { get; set; }
        public List<EntryModel> Entries { get; set; }
    }
}