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
using eclectica.co.uk.Web.Abstract;
using System.Net;
using eclectica.co.uk.Caching.Abstract;

namespace eclectica.co.uk.Web.Controllers
{
    public class TemplateController : BaseController
    {
        private ITagServices _tagServices;
        private ILinkServices _linkServices;

        public TemplateController(IEntryServices entryServices, ICommentServices commentServices, IConfigurationInfo config, ITagServices tagServices, ILinkServices linkServices) : base(entryServices, commentServices, config) 
        {
            _tagServices = tagServices;
            _linkServices = linkServices;
        }

        public ActionResult About(bool mobile = false)
        {
            return View(mobile);
        }

        public ActionResult Links(bool mobile = false)
        {
            return View(new LinksViewModel { 
                Mobile = mobile,
                Links = _linkServices.GetSortedLinks().ToList()
            });
        }

        public ActionResult Tags(string tagName, bool mobile = false)
        {
            if (tagName != null) // Show list of entries for this tag
            {
                IDictionary<string, List<EntryModel>> entryDictionary;

                entryDictionary = _entryServices.GetEntriesForTag(tagName);
                // This is approx 100x slower - why?
                // entryDictionary = _entryServices.GetEntriesForTag(tagName);

                return View("TagEntries", new TagEntriesViewModel {
                    Mobile = mobile,
                    TagName = tagName.Capitalise(),
                    EntryDictionary = entryDictionary
                });

            }
            else // Show weighted tag cloud
            {

                Dictionary<string, List<TagModel>> tagDictionary;

                tagDictionary = _tagServices.GetSortedTags();
                
                return View("TagIndex", new TagIndexViewModel {
                    Mobile = mobile,
                    TagDictionary = tagDictionary
                });

            }
        }
    }
}
