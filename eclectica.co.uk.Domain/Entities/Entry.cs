﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace eclectica.co.uk.Domain.Entities
{
    public class Entry
    {
        public int EntryID { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime Published { get; set; }
        public DateTime Updated { get; set; }
        public string Body { get; set; }
        public string Tweet { get; set; }
        public bool Publish { get; set; }

        public virtual Author Author { get; set; }

        public int CommentCount { get; set; }
        public string Thumbnail { get; set; }
        public string LargeThumbnail { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Entry> Related { get; set; }
    }
}
