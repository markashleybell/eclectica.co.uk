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
        public CommentController(IEntryServices entryServices, ICommentServices commentServices, IConfigurationInfo config) : base(entryServices, commentServices, config) { }

        [Authorize]
        public ActionResult Manage(int? page)
        {
            return View(new CommentManageViewModel {
                Comments = _commentServices.All().ToList()
            });
        }

        [Authorize]
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

        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(CommentEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Comments = _commentServices.All()
                                                 .Select(x => new SelectListItem {
                                                     Text = x.Body.StripHtml().Truncate(50),
                                                     Value = x.CommentID.ToString()
                                                 }).AsQueryable();
                return View(model);
            }

            _commentServices.UpdateComment(model.Comment);

            return RedirectToAction("Edit", new { id = model.Comment.CommentID });
        }

        // TODO: Rework delete buttons into POSTs
        // [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            _commentServices.DeleteComment(id);

            return RedirectToAction("Manage");
        }
    }
}
