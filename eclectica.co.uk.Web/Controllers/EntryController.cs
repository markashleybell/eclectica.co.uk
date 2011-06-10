using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eclectica.co.uk.Service.Abstract;
using eclectica.co.uk.Web.Models;

namespace eclectica.co.uk.Web.Controllers
{
    public class EntryController : BaseController
    {
        public EntryController(IEntryServices entryServices, ICommentServices commentServices) : base(entryServices, commentServices) { }

        public ActionResult Index()
        {
            return View(new IndexViewModel { 
                Entries = _entryServices.All().ToList()
            });
        }

    }
}
