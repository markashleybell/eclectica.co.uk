using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace eclectica.co.uk.Service.Entities
{
    public class EntryModel
    {
        public int EntryID { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime Published { get; set; }
        public DateTime Updated { get; set; }

        public string Body { get; set; }
        public string Tweet { get; set; }
        public bool Publish { get; set; }

        public string Thumbnail { get; set; }
        public string LargeThumbnail { get; set; }

        public int CommentCount { get; set; }

        public AuthorModel Author { get; set; }

        public List<CommentModel> Comments { get; set; }
        public List<TagModel> Tags { get; set; }
        public List<EntryModel> Related { get; set; }
    }
}
