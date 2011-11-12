using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eclectica.co.uk.Service.Entities;

namespace eclectica.co.uk.Web.Models
{
    public class SearchResultsViewModel : BaseViewModel
    {
        public string Query { get; set; }
        public List<EntryModel> SearchResults { get; set; }
        public bool CreateIndex { get; set; }
    }
}