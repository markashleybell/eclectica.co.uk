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
    public class RedirectController : BaseController
    {
        private IRedirectServices _redirectServices;

        public RedirectController(IEntryServices entryServices, ICommentServices commentServices, IConfigurationInfo config, IRedirectServices redirectServices) : base(entryServices, commentServices, config) 
        {
            _redirectServices = redirectServices;
        }

        [Authorize]
        public ActionResult Manage(int? page)
        {
            return View(new RedirectManageViewModel {
                Redirects = _redirectServices.All().ToList()
            });
        }

        [Authorize]
        public ActionResult Create()
        {
            return View(new RedirectEditViewModel {
                Redirects = _redirectServices.All()
                                     .Select(x => new SelectListItem {
                                         Text = x.RedirectUrl.Truncate(60),
                                         Value = x.RedirectID.ToString()
                                     }).AsQueryable()
            });
        }

        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(RedirectEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Redirects = _redirectServices.All()
                                           .Select(x => new SelectListItem {
                                               Text = x.RedirectUrl.Truncate(60),
                                               Value = x.RedirectID.ToString()
                                           }).AsQueryable();
                return View(model);
            }

            _redirectServices.AddRedirect(model.Redirect);

            return RedirectToAction("Manage");
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var redirect = _redirectServices.GetRedirect(id);

            return View(new RedirectEditViewModel {
                Redirect = redirect,
                Redirects = _redirectServices.All()
                                     .Select(x => new SelectListItem {
                                         Text = x.RedirectUrl.Truncate(60),
                                         Value = x.RedirectID.ToString()
                                     }).AsQueryable()
            });
        }

        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(RedirectEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Redirects = _redirectServices.All()
                                           .Select(x => new SelectListItem {
                                               Text = x.RedirectUrl.Truncate(60),
                                               Value = x.RedirectID.ToString()
                                           }).AsQueryable();
                return View(model);
            }

            _redirectServices.UpdateRedirect(model.Redirect);

            return RedirectToAction("Manage");
        }

        // TODO: Rework delete buttons into POSTs
        // [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            _redirectServices.DeleteRedirect(id);

            return RedirectToAction("Manage");
        }

        public RedirectResult Redirect(string id)
        {
            var redirect = _redirectServices.GetRedirect(Int32.Parse(id));
            
            if(redirect == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "Page not found");

            redirect.Clicks++;

            _redirectServices.UpdateRedirect(redirect);

            return RedirectPermanent(redirect.RedirectUrl);
        }
    }
}
