using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eclectica.co.uk.Service.Abstract;
using eclectica.co.uk.Web.Models;

namespace eclectica.co.uk.Web.Controllers
{
    
    public class AdminController : BaseController
    {
        IFormsAuthenticationProvider _auth;

        public AdminController(IEntryServices entryServices, ICommentServices commentServices, ITagServices tagServices, ILinkServices linkServices, IFormsAuthenticationProvider auth) : base(entryServices, commentServices, tagServices, linkServices) 
        {
            _auth = auth;
        }

        [Authorize]
        public ActionResult Posts()
        {
            return View();
        }

        [Authorize]
        public ActionResult Comments()
        {
            return View();
        }

        [Authorize]
        public ActionResult Links()
        {
            return View();
        }

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
                if (!_auth.Authenticate(model.Username, model.Password))
                    ModelState.AddModelError("", "Incorrect username or password");

            if (ModelState.IsValid)
            {
                _auth.SetAuthCookie(model.Username, false);
                return Redirect(returnUrl ?? "/admin/posts");
            }
            else
            {
                return View();
            }
        }

    }
}
