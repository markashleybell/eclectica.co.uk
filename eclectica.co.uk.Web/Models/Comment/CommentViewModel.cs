using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using eclectica.co.uk.Web.Infrastructure;

namespace eclectica.co.uk.Web.Models
{
    public class CommentViewModel
    {
        [Required]
        public int EntryID { get; set; }

        [Required]
        public string EntryUrl { get; set; }

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

        [Required(ErrorMessage = "Hint. It's five. Or 5, if you prefer.")]
        [MustEqual("Hint. It's five. Or 5, if you prefer.", new string[] { "5", "five" }, true)]
        [DisplayName("How many monkeys in a bag of five monkeys?")]
        public string X7fw91Do { get; set; }

        public int Error { get; set; }
    }
}