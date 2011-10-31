using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eclectica.co.uk.Service.Abstract;
using eclectica.co.uk.Web.Models;
using eclectica.co.uk.Web.Extensions;
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

        public ActionResult Index(int? page)
        {
            List<EntryModel> entries;

            var currentPage = (page.HasValue) ? page.Value : 0;
            var pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["HomePagePostCount"]);
            
            entries = _entryServices.Page((pageSize * currentPage), pageSize).ToList();

            return View(new IndexViewModel {
                Entries = entries,
                PageSize = pageSize,
                CurrentPage = currentPage
            });
        }

        public ActionResult Manage(int? page)
        {
            return View(new AdminEntriesViewModel {
                Entries = _entryServices.All().ToList()
            });
        }

        public ActionResult Archive(int year, int month)
        {
            List<EntryModel> entries;
            IDictionary<DateTime, int> months;

            months = _entryServices.GetPostCountsPerMonth(year);
            
            entries = _entryServices.GetArchivedEntries(year, month).ToList();

            return View(new ArchiveViewModel { 
                Date = new DateTime(year, month, 1),
                Months = months,
                Entries = entries
            });
        }

        public ActionResult Create()
        {
            return View(new EntryEditViewModel {
                Entry = new EntryModel(),
                Images = _entryServices.GetImages(),
                Tags = ""
            });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(EntryEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _entryServices.AddEntry(model.Entry);

            return RedirectToAction("Edit", new { id = model.Entry.EntryID });
        }

        public ActionResult Edit(int id)
        {
            var entry = _entryServices.GetEntry(id);

            return View(new EntryEditViewModel { 
                Entry = entry,
                Images = _entryServices.GetImages(),
                Tags = string.Join(" ", entry.Tags.Select(x => x.TagName).ToArray())
            });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(EntryEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _entryServices.UpdateEntry(model.Entry);

            return RedirectToAction("Edit", new { id = model.Entry.EntryID });
        }

        public ActionResult Detail(string url)
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
        public ActionResult Detail(CommentViewModel comment)
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

        // TODO: Rework delete buttons into POSTs
        // [HttpPost]
        public ActionResult Delete(int id)
        {
            _entryServices.DeleteEntry(id);

            return RedirectToAction("Manage");
        }

        public ActionResult Random()
        {
            var url = _entryServices.GetRandomEntryUrl();

            //return View("Debug");
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
