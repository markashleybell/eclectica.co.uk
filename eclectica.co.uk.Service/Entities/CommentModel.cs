using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace eclectica.co.uk.Service.Entities
{
    public class CommentModel
    {
        public int CommentID { get; set; }

        public EntryModel Entry { get; set; }

        [Required(ErrorMessage = "You must enter your name.")]
        [DisplayName("Your name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You must enter your email address.")]
        [RegularExpression(@"(?i)^[a-z0-9!#$%&\'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&\'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+(?:[A-Z]{2}|com|org|net|gov|mil|biz|info|mobi|name|aero|jobs|museum|edu)\b", ErrorMessage = "The address you gave was not valid.")]
        [DisplayName("Your email address")]
        public string Email { get; set; }

        [DisplayName("Your web site (optional)")]
        public string Url { get; set; }

        [Required(ErrorMessage = "You must enter a comment.")]
        [DisplayName("Comment (basic HTML is allowed)")]
        public string RawBody { get; set; }

        public string Body { get; set; }
        public bool Approved { get; set; }

        public DateTime Date { get; set; }
 
    }
}
