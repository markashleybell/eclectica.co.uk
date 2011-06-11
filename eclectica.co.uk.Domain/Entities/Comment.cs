﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace eclectica.co.uk.Domain.Entities
{
    public class Comment
    {
        public int CommentID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }
        public DateTime Date { get; set; }
        [Column(TypeName = "ntext")]
        public string Body { get; set; }
        [Column(TypeName = "ntext")]
        public string RawBody { get; set; }
        public bool Approved { get; set; }

        public virtual Entry Entry { get; set; }
    }
}