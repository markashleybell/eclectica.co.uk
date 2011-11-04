using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eclectica.co.uk.Service.Entities;
using System.ComponentModel.DataAnnotations;
using eclectica.co.uk.Web.Infrastructure;
using System.ComponentModel;

namespace eclectica.co.uk.Web.Models
{
    public class EntryViewModel
    {
        public EntryModel Entry { get; set; }
        public CommentModel Comment { get; set; }

        [Required(ErrorMessage = "Hint. It's five. Or 5, if you prefer.")]
        [MustEqual("Hint. It's five. Or 5, if you prefer.", new string[] { "5", "five" }, true)]
        [DisplayName("How many monkeys in a bag of five monkeys?")]
        public string X7fw91Do { get; set; }

    }
}