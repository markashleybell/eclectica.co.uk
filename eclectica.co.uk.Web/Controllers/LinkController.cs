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
    public class LinkController : BaseController
    {
        public LinkController(IEntryServices entryServices, ICommentServices commentServices, ITagServices tagServices, ILinkServices linkServices, IConfigurationInfo config) : base(entryServices, commentServices, tagServices, linkServices, config) { }

        public ActionResult Manage(int? page)
        {
            return View(new LinkManageViewModel {
                Links = _linkServices.All().ToList()
            });
        }

       

        public ActionResult Create()
        {
            return View(new LinkEditViewModel {
                Links = _linkServices.All()
                                     .Select(x => new SelectListItem {
                                         Text = x.Title,
                                         Value = x.LinkID.ToString()
                                     }).AsQueryable()
            });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(LinkEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Links = _linkServices.All()
                                           .Select(x => new SelectListItem {
                                               Text = x.Title,
                                               Value = x.LinkID.ToString()
                                           }).AsQueryable();
                return View(model);
            }

            _linkServices.AddLink(model.Link);

            return RedirectToAction("Edit", new { id = model.Link.LinkID });
        }

        public ActionResult Edit(int id)
        {
            var link = _linkServices.GetLink(id);

            return View(new LinkEditViewModel {
                Link = link,
                Links = _linkServices.All()
                                     .Select(x => new SelectListItem {
                                         Text = x.Title,
                                         Value = x.LinkID.ToString()
                                     }).AsQueryable()
            });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(LinkEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Links = _linkServices.All()
                                           .Select(x => new SelectListItem {
                                               Text = x.Title,
                                               Value = x.LinkID.ToString()
                                           }).AsQueryable();
                return View(model);
            }

            _linkServices.UpdateLink(model.Link);

            return RedirectToAction("Edit", new { id = model.Link.LinkID });
        }

        // TODO: Rework delete buttons into POSTs
        // [HttpPost]
        public ActionResult Delete(int id)
        {
            _linkServices.DeleteLink(id);

            return RedirectToAction("Manage");
        }
    }
}
