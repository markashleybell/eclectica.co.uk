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
using System.IO;
using eclectica.co.uk.Web.Abstract;
using mab.lib.ImageSizer;

namespace eclectica.co.uk.Web.Controllers
{
    public class EntryController : BaseController
    {
        public EntryController(IEntryServices entryServices, ICommentServices commentServices, ITagServices tagServices, ILinkServices linkServices, IConfigurationInfo config) : base(entryServices, commentServices, tagServices, linkServices, config) { }

        public ActionResult Index(int? page, string view)
        {
            List<EntryModel> entries;

            var currentPage = (page.HasValue) ? page.Value : 0;
            var pageSize = (view == null) ? _config.IndexPageSize : 10;
            
            entries = _entryServices.Page((pageSize * currentPage), pageSize).ToList();

            return View(((view == null) ? "Index" : view), new IndexViewModel {
                Entries = entries,
                PageSize = pageSize,
                CurrentPage = currentPage
            });
        }

        [Authorize]
        public ActionResult CreateSearchIndex()
        {
            _entryServices.CreateSearchIndex();

            return View();
        }

        [Authorize]
        public ActionResult Manage(int? page)
        {
            return View(new EntryManageViewModel {
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

        [Authorize]
        public ActionResult Create()
        {
            return View(new EntryEditViewModel {
                Images = _entryServices.GetImages(),
                Entries = _entryServices.All()
                                        .Select(x => new SelectListItem {
                                            Text = x.Published.ToString("dd/MM/yyyy hh:mm") + " " + x.Title.Truncate(50),
                                            Value = x.EntryID.ToString()
                                        }).AsQueryable()
            });
        }

        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(EntryEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Images = _entryServices.GetImages();
                model.Entries = _entryServices.All()
                                              .Select(x => new SelectListItem {
                                                  Text = x.Published.ToString("dd/MM/yyyy hh:mm") + " " + x.Title.Truncate(50),
                                                  Value = x.EntryID.ToString()
                                              }).AsQueryable();
                return View(model);
            }

            _entryServices.AddEntry(model.Entry,
                                    ((model.Related == null) ? null : model.Related.Split('|').Select(x => Convert.ToInt32(x)).ToArray()),
                                    model.Tags.SplitOrNull(" "));

            return RedirectToAction("Edit", new { id = model.Entry.EntryID });
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var entry = _entryServices.GetEntry(id);

            return View(new EntryEditViewModel { 
                Entry = entry,
                Images = _entryServices.GetImages(),
                Tags = string.Join(" ", entry.Tags.Select(x => x.TagName).ToArray()),
                Related = string.Join("|", entry.Related.Select(x => x.EntryID).ToArray()),
                Entries = _entryServices.All()
                                        .Select(x => new SelectListItem { 
                                            Text = x.Published.ToString("dd/MM/yyyy hh:mm") + " " + x.Title.Truncate(50), 
                                            Value = x.EntryID.ToString()
                                        }).AsQueryable()
            });
        }

        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(EntryEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Images = _entryServices.GetImages();
                model.Entries = _entryServices.All()
                                              .Select(x => new SelectListItem {
                                                  Text = x.Published.ToString("dd/MM/yyyy hh:mm") + " " + x.Title.Truncate(50),
                                                  Value = x.EntryID.ToString()
                                              }).AsQueryable();
                return View(model);
            }

            _entryServices.UpdateEntry(model.Entry,
                                       ((model.Related == null) ? null : model.Related.Split('|').Select(x => Convert.ToInt32(x)).ToArray()),
                                       model.Tags.SplitOrNull(" "));

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
                Comment = new CommentModel {
                    Entry = entry
                }
            });
        }

        [HttpPost]
        public ActionResult Detail(EntryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Entry = _entryServices.GetEntryByUrl(model.Entry.Url);

                return View(model);
            }

            model.Comment.Entry = model.Entry;

            _commentServices.AddComment(model.Comment);

            return Redirect("/" + model.Entry.Url + "#comment" + model.Comment.CommentID);
        }

        // TODO: Rework delete buttons into POSTs
        // [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            _entryServices.DeleteEntry(id);

            return RedirectToAction("Manage");
        }

        [Authorize]
        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase upload)
        {
            var extension = Path.GetExtension(upload.FileName).ToLower();
            var fileName = Path.GetFileName(upload.FileName);
            var image = new ImageModel { Filename = fileName };

            if (extension == ".jpg")
            {
                _entryServices.AddImage(image);

                // Resize and save the physical image
                var libraryPath = _config.ImageLibraryFolder;

                var imageSizer = new ImageSizer();

                imageSizer.SaveImageCropToDimensions(upload, libraryPath + "\\crop\\" + image.ImageID + extension, 74, 74, CropAnchorPosition.Center, 85);
                imageSizer.SaveImageWithMaxDimensions(upload, libraryPath + "\\cms\\" + image.ImageID + extension, 90, 90, 50);

                upload.SaveAs(libraryPath + "\\std\\" + image.ImageID + extension);
            }

            return Content(fileName + ":" + image.ImageID);
        }

        [Authorize]
        public ActionResult RelatedSearch(string query)
        {
            var entries = _entryServices.SimpleSearch(query);

            if(entries == null)
                return Content("");

            var matches = (from e in entries
                           select e.EntryID + "^" + e.Title + "^" + e.Published.ToString("dd/MM/yyyy hh:mm")).ToArray();

            return Content(string.Join("|", matches));
        }

        [Authorize]
        public ActionResult TagSearch(string query)
        {
            var tags = _tagServices.Search(query);

            if (tags == null)
                return Content("");

            var matches = (from t in tags
                           select t.TagName).ToArray();

            return Content(string.Join("|", matches));
        }

        [Authorize]
        public ActionResult Unpublish(int id)
        {
            var entry = _entryServices.GetEntry(id);
            entry.Publish = false;

            _entryServices.UpdateEntry(entry, null, null);

            return RedirectToAction("Manage");
        }

        [Authorize]
        public ActionResult Publish(int id)
        {
            var entry = _entryServices.GetEntry(id);
            entry.Publish = true;

            _entryServices.UpdateEntry(entry, null, null);

            return RedirectToAction("Manage");
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
