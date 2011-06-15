﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eclectica.co.uk.Service.Abstract;
using eclectica.co.uk.Web.Models;
using eclectica.co.uk.Web.Extensions;
using MvcMiniProfiler;
using eclectica.co.uk.Service.Entities;
using System.Configuration;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace eclectica.co.uk.Web.Controllers
{
    public class EntryController : BaseController
    {
        public EntryController(IEntryServices entryServices, ICommentServices commentServices) : base(entryServices, commentServices) { }

        public ActionResult Index(int? page)
        {
            var profiler = MiniProfiler.Current; // it's ok if this is null

            List<EntryModel> entries;

            var currentPage = (page.HasValue) ? page.Value : 0;
            var pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["HomePagePostCount"]);

            using(profiler.Step("Loading last 5 entries"))
            {
                entries = _entryServices.Page((pageSize * currentPage), pageSize).ToList();
            }

            return View(new IndexViewModel {
                Entries = entries,
                PageSize = pageSize,
                CurrentPage = currentPage
            });
        }

        public ActionResult RecentEntries()
        {
            return View(new RecentEntriesViewModel { 
                Entries = _entryServices.GetRecentEntries(10).ToList()
            });
        }

        [HttpPost]
        [OutputCache(Duration=300)]
        public ActionResult RecentTwitterStatuses(int count)
        {
            var url = "https://api.twitter.com/1/statuses/user_timeline.json?screen_name=eclecticablog&count=" + count;

            WebClient w = new WebClient();
            var json = w.DownloadString(url);

            JArray o = JArray.Parse(json);


            var statuses = from s in o
                           select new 
                           {
                               date = s["created_at"].ToString().ToPrettyDate(),
                               status = s["text"].ToString()
                           };

            return Json(statuses);
        }
    }
}
