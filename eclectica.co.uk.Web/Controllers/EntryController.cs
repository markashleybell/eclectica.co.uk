using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eclectica.co.uk.Service.Abstract;
using eclectica.co.uk.Web.Models;
using MvcMiniProfiler;
using eclectica.co.uk.Service.Entities;

namespace eclectica.co.uk.Web.Controllers
{
    public class EntryController : BaseController
    {
        public EntryController(IEntryServices entryServices, ICommentServices commentServices) : base(entryServices, commentServices) { }

        public ActionResult Index(int? page)
        {
            var profiler = MiniProfiler.Current; // it's ok if this is null

            List<EntryModel> entries;

            using(profiler.Step("Loading last 5 entries"))
            {
                 entries = _entryServices.Last(5).ToList();
            }

            return View(new IndexViewModel {
                Entries = entries
            });
        }

    }
}
