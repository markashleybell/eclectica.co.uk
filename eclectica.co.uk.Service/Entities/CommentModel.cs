using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace eclectica.co.uk.Service.Entities
{
    public class CommentModel
    {
        public int CommentID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }
        public DateTime Date { get; set; }
        public string Body { get; set; }
        public string RawBody { get; set; }
        public bool Approved { get; set; }
    }
}
