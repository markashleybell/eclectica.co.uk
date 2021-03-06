﻿using System;
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
using Elmah.Contrib.Mvc;
using eclectica.co.uk.Caching.Abstract;
using LinqToTwitter;

namespace eclectica.co.uk.Web.Controllers
{
    public class EntryController : BaseController
    {
        private ITagServices _tagServices;
        private IModelCache _cache;

        public EntryController(IEntryServices entryServices, ICommentServices commentServices, ITagServices tagServices, IConfigurationInfo config, IModelCache cache) : base(entryServices, commentServices, config) 
        {
            _tagServices = tagServices;
            _cache = cache;
        }

        public ActionResult Index(int? page, string view, bool mobile = false)
        {
            List<EntryModel> entries;

            var currentPage = (page.HasValue) ? page.Value : 0;
            var pageSize = (view == null) ? _config.IndexPageSize : 10;
            
            entries = _entryServices.Page((pageSize * currentPage), pageSize).ToList();

            return View(((view == null) ? "Index" : view), new IndexViewModel {
                Mobile = mobile,
                Entries = entries,
                PageSize = pageSize,
                CurrentPage = currentPage
            });
        }

        public ActionResult XmlSiteMap()
        {
            return View(_entryServices.GetUrlList());
        }

        public ActionResult PostToTwitter()
        {
            throw new HttpException((int)HttpStatusCode.NotFound, _config.Error404Message);
        }

        [Authorize]
        [HttpPost]
        public ActionResult PostToTwitter(string url, string text)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(text))
                return Json(new { Success = false, Message = "You must enter some text to tweet" });

            IOAuthCredentials credentials = new SessionStateCredentials();

            credentials.ConsumerKey = _config.TwitterConsumerKey;
            credentials.ConsumerSecret = _config.TwitterConsumerSecret;
            credentials.AccessToken = _config.TwitterAccessTokenSecret;
            credentials.OAuthToken = _config.TwitterAccessToken;

            MvcAuthorizer auth = new MvcAuthorizer { Credentials = credentials };

            TwitterContext twitter = new TwitterContext(auth);

            try
            {
                twitter.UpdateStatus(text + " " + url);

                return Json(new { Success = true });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, Message = e.Message });
            }
        }

        [Authorize]
        public ActionResult CreateSearchIndex()
        {
            _entryServices.CreateSearchIndex();

            return View();
        }

        [Authorize]
        public ActionResult ClearErrorLogs()
        {
            return View(new ClearErrorLogViewModel { 
                Limit = DateTime.Now
            });
        }

        [Authorize]
        [HttpPost]
        public ActionResult ClearErrorLogs(ClearErrorLogViewModel model)
        {
            _entryServices.ClearErrorLogs(model.Limit);

            model.Status = "Logs cleared up to " + model.Limit.ToString("dd/MM/yyyy hh:mm");

            return View(model);
        }

        [Authorize]
        public ActionResult Manage(bool? latest = true)
        {
            return View(new EntryManageViewModel {
                Entries = _entryServices.Manage(latest).ToList()
            });
        }

        public ActionResult Archive(int year, int month, bool mobile = false)
        {
            List<EntryModel> entries;
            IDictionary<DateTime, int> months;

            months = _entryServices.GetPostCountsPerMonth(year);
            
            entries = _entryServices.GetArchivedEntries(year, month).ToList();

            return View(new ArchiveViewModel { 
                Mobile = mobile,
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
                FacebookApplicationID = _config.FacebookApplicationID,
                FacebookPageID = _config.FacebookPageID,
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
                model.FacebookApplicationID = _config.FacebookApplicationID;
                model.FacebookPageID = _config.FacebookPageID;

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

        public ActionResult Preview(int id, bool mobile = false)
        {
            var entry = _entryServices.GetEntry(id);

            if (entry == null)
                throw new HttpException((int)HttpStatusCode.NotFound, _config.Error404Message);

            return View("Detail", new EntryViewModel {
                Mobile = mobile,
                Entry = entry,
                Comment = new CommentModel {
                    Entry = entry
                }
            });
        }

        public ActionResult Detail(string url, bool mobile = false)
        {
            var entry = _entryServices.GetEntryByUrl(url);

            if (entry == null)
                throw new HttpException((int)HttpStatusCode.NotFound, _config.Error404Message);

            return View(new EntryViewModel { 
                Mobile = mobile,
                Entry = entry,
                Comment = new CommentModel {
                    Entry = entry
                }
            });
        }

        [HttpPost]
        public ActionResult Detail(EntryViewModel model, bool mobile = false)
        {
            if (model.IsAjaxRequest)
            {
                return Json(ModelState.GetErrorsForJSON());
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    model.Entry = _entryServices.GetEntryByUrl(model.Entry.Url);

                    return View(model);
                }
            }

            model.Comment.Entry = model.Entry;

            _commentServices.AddComment(model.Comment);

            return Redirect("/" + model.Entry.Url + "#comment-form");
        }

        // TODO: Rework delete buttons into POSTs
        // [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            _entryServices.DeleteEntry(id);

            return RedirectToAction("Manage");
        }

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

        /// <summary>
        /// A quick search for entries used in the CMS when finding related entries
        /// </summary>
        /// <param name="query">Search query</param>
        /// <returns>String of search results</returns>
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
            var urls = _entryServices.GetEntryUrls();

            var rnd = new Random();

            var url = urls[rnd.Next(0, urls.Length - 1)];

            return Redirect("/" + url);
        }

        public ActionResult RecentEntries()
        {
            return View(new RecentEntriesViewModel { 
                Entries = _entryServices.GetRecentEntries(10).ToList()
            });
        }

        public ActionResult RecentTwitterStatuses()
        {
            throw new HttpException((int)HttpStatusCode.NotFound, _config.Error404Message);
        }

        [HttpPost]
        [OutputCache(Duration=1800)]
        public ActionResult RecentTwitterStatuses(int? count)
        {
            if(!count.HasValue)
                throw new HttpException((int)HttpStatusCode.NotFound, _config.Error404Message);

            var url = "https://api.twitter.com/1/statuses/user_timeline.json?screen_name=eclecticablog&count=" + count.Value;

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

        public ActionResult Search(SearchResultsViewModel model, bool? ajax, bool mobile = false)
        {
            // Check that the Lucene index exists, and if not create it
            var indexPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Index");

            if (!Directory.Exists(indexPath) || model.CreateIndex)
                _entryServices.CreateSearchIndex();

            var results = _entryServices.SearchEntries(model.Query);

            model.Mobile = mobile;
            model.SearchResults = results.ToList();

            // If this is an AJAX search just return the result data directly 
            // as JSON, otherwise show the results view
            if (ajax.HasValue && ajax.Value == true)
                return Json(model.SearchResults, JsonRequestBehavior.AllowGet);
            else
                return View("SearchResults", model);
        }

        [Authorize]
        public ActionResult ShowCacheContents()
        {
            return View(_cache.BaseCache.OrderByDescending(x => x.Hits).ToList());
        }

        [Authorize]
        public ActionResult ClearCacheContents()
        {
            _cache.Clear();

            return RedirectToAction("ShowCacheContents");
        }
    }
}
