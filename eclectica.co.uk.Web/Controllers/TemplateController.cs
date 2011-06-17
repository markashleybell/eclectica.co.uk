﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eclectica.co.uk.Service.Abstract;
using eclectica.co.uk.Web.Models;
using eclectica.co.uk.Service.Entities;
using MvcMiniProfiler;
using eclectica.co.uk.Web.Extensions;

namespace eclectica.co.uk.Web.Controllers
{
    public class TemplateController : BaseController
    {
        public TemplateController(IEntryServices entryServices, ICommentServices commentServices, ITagServices tagServices) : base(entryServices, commentServices, tagServices) { }

        private MiniProfiler profiler = MiniProfiler.Current;

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Search(SearchResultsViewModel model)
        {
            if (model.CreateIndex)
                _entryServices.CreateSearchIndex();

            var results = _entryServices.SearchEntries(model.Query);

            model.SearchResults = results.ToList();

            return View("SearchResults", model);
        }

        public ActionResult Tags(string tagName)
        {
            if (tagName != null) // Show list of entries for this tag
            {
                Dictionary<string, List<EntryModel>> entryDictionary;

                using (profiler.Step("Get entries for specific tag (" + tagName + ")"))
                {
                    entryDictionary = _tagServices.GetEntriesForTag(tagName);
                    // This is approx 100x slower - why?
                    // entryDictionary = _entryServices.GetEntriesForTag(tagName);
                }

                return View("TagEntries", new TagEntriesViewModel
                { 
                    TagName = tagName.Capitalise(),
                    EntryDictionary = entryDictionary
                });

            }
            else // Show weighted tag cloud
            {

                Dictionary<string, List<TagModel>> tagDictionary;

                using (profiler.Step("Loading all tags (+ use counts)"))
                {
                    tagDictionary = _tagServices.GetSortedTags();
                }

                return View("TagIndex", new TagIndexViewModel
                {
                    TagDictionary = tagDictionary
                });

            }
        }

    }
}
