using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eclectica.co.uk.Service.Entities
{
    public class RedirectModel
    {
        public int RedirectID { get; set; }
        public string RedirectUrl { get; set; }
        public int Clicks { get; set; }
    }
}
