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
    public class CommentController : BaseController
    {
        public CommentController(IEntryServices entryServices, ICommentServices commentServices, ITagServices tagServices, ILinkServices linkServices, IConfigurationInfo config) : base(entryServices, commentServices, tagServices, linkServices, config) { }

        public ActionResult Manage(int? page)
        {
            return View(new CommentManageViewModel {
                Comments = _commentServices.All().ToList()
            });
        }

       

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

        public ActionResult Edit(int id)
        {
            var comment = _commentServices.GetComment(id);

            return View(new CommentEditViewModel {
                Comment = comment,
                Comments = _commentServices.All()
                                           .Select(x => new SelectListItem { 
                                               Text = x.Body.StripHtml().Truncate(50), 
                                               Value = x.CommentID.ToString()
                                           }).AsQueryable()
            });
        }

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

        // TODO: Rework delete buttons into POSTs
        // [HttpPost]
        public ActionResult Delete(int id)
        {
            _entryServices.DeleteEntry(id);

            return RedirectToAction("Manage");
        }
    }
}
