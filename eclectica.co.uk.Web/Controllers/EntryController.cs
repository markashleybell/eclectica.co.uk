using System;
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
        public EntryController(IEntryServices entryServices, ICommentServices commentServices, ITagServices tagServices, ILinkServices linkServices) : base(entryServices, commentServices, tagServices, linkServices) { }

        private MiniProfiler profiler = MiniProfiler.Current;

        public ActionResult Index(int? page)
        {
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

        public ActionResult Entry(string url)
        {
            var entry = _entryServices.GetEntryByUrl(url);

            if (entry == null)
            {
                return Content("404");
            }

            return View(new EntryViewModel { 
                Entry = entry,
                Comment = new CommentViewModel {
                    EntryID = entry.EntryID,
                    EntryUrl = entry.Url
                }
            });
        }

        [HttpPost]
        public ActionResult Entry(CommentViewModel comment)
        {
            if (!ModelState.IsValid)
            {
                var entry = _entryServices.GetEntryByUrl(comment.EntryUrl);

                if (entry == null)
                {
                    return Content("404");
                }

                return View(new EntryViewModel
                {
                    Entry = entry,
                    Comment = comment
                });
            }

            // Add comment
            int commentId = _commentServices.AddComment(comment.EntryID, comment.Name, comment.Email, comment.Url, comment.RawBody);

            return Redirect("/" + comment.EntryUrl + "#comment" + commentId);
        }

        public ActionResult Random()
        {
            var url = _entryServices.GetRandomEntryUrl();

            return Redirect("/" + url);
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
