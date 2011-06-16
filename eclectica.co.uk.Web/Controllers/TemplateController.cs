using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eclectica.co.uk.Service.Abstract;
using eclectica.co.uk.Web.Models;
using eclectica.co.uk.Service.Entities;
using MvcMiniProfiler;

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

        public ActionResult Tags(string tag)
        {
            var tagDictionary = new Dictionary<string, List<TagModel>>();

            List<TagModel> allTags;

            using (profiler.Step("Loading all tags (+ use counts)"))
            {
                allTags = _tagServices.All().ToList();
            }

            using (profiler.Step("Sorting tags"))
            {
                foreach (var t in allTags)
                {
                    var first = t.TagName[0].ToString().ToUpper();

                    if (!tagDictionary.ContainsKey(first))
                        tagDictionary.Add(first, new List<TagModel>());

                    tagDictionary[first].Add(t);
                }
            }

            return View(new TagsViewModel {
                TagDictionary = tagDictionary
            });
        }

    }
}
