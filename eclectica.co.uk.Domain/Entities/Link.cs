﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eclectica.co.uk.Domain.Entities
{
    public class Link
    {
        public int LinkID { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Category { get; set; }
    }
}
