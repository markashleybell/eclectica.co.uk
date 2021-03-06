﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eclectica.co.uk.Service.Entities;

namespace eclectica.co.uk.Web.Models
{
    public class TagEntriesViewModel : BaseViewModel
    {
        public string TagName { get; set; }
        public IDictionary<string, List<EntryModel>> EntryDictionary { get; set; }
    }
}