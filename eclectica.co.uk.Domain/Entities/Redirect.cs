using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eclectica.co.uk.Domain.Entities
{
    public class Redirect
    {
        public int RedirectID { get; set; }
        public string RedirectUrl { get; set; }
        public int Clicks { get; set; }
    }
}
