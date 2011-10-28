using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eclectica.co.uk.Service.Abstract;
using eclectica.co.uk.Web.Models;
using eclectica.co.uk.Service.Entities;
using eclectica.co.uk.Web.Extensions;
using System.IO;

namespace eclectica.co.uk.Web.Controllers
{
    public class TemplateController : BaseController
    {
        public TemplateController(IEntryServices entryServices, ICommentServices commentServices, ITagServices tagServices, ILinkServices linkServices) : base(entryServices, commentServices, tagServices, linkServices) { }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Search(SearchResultsViewModel model, bool? ajax)
        {
            var indexPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Index");

            if (!Directory.Exists(indexPath) || model.CreateIndex)
                _entryServices.CreateSearchIndex();

            var results = _entryServices.SearchEntries(model.Query);

            model.SearchResults = results.ToList();

            if (ajax.HasValue && ajax.Value == true)
                return Json(model, JsonRequestBehavior.AllowGet);
            else
                return View("SearchResults", model);
        }

        public ActionResult Links()
        {
            return View(new LinksViewModel { 
                Links = _linkServices.GetSortedLinks().ToList()
            });
        }

        public ActionResult Tags(string tagName)
        {
            if (tagName != null) // Show list of entries for this tag
            {
                IDictionary<string, List<EntryModel>> entryDictionary;

                entryDictionary = _entryServices.GetEntriesForTag(tagName);
                // This is approx 100x slower - why?
                // entryDictionary = _entryServices.GetEntriesForTag(tagName);

                return View("TagEntries", new TagEntriesViewModel
                { 
                    TagName = tagName.Capitalise(),
                    EntryDictionary = entryDictionary
                });

            }
            else // Show weighted tag cloud
            {

                Dictionary<string, List<TagModel>> tagDictionary;

                tagDictionary = _tagServices.GetSortedTags();
                
                return View("TagIndex", new TagIndexViewModel
                {
                    TagDictionary = tagDictionary
                });

            }
        }

    }
}
