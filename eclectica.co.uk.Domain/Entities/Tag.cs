using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eclectica.co.uk.Domain.Entities
{
    public class Tag
    {
        public int TagID { get; set; }
        public string TagName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Entry> Entries { get; set; }
    }
}
