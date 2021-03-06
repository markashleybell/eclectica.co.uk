﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eclectica.co.uk.Service.Abstract;
using eclectica.co.uk.Web.Models;
using eclectica.co.uk.Web.Abstract;

namespace eclectica.co.uk.Web.Controllers
{
    
    public class AuthController : BaseController
    {
        IFormsAuthenticationProvider _auth;

        public AuthController(IEntryServices entryServices, ICommentServices commentServices, IConfigurationInfo config, IFormsAuthenticationProvider auth)
            : base(entryServices, commentServices, config) 
        {
            _auth = auth;
        }

        public ActionResult LogOn(string returnUrl)
        {
            return View(new LogOnViewModel { 
                ReturnUrl = returnUrl
            });
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
                return Redirect(returnUrl ?? "/Entry/Manage");
            }
            else
            {
                return View();
            }
        }

        public ActionResult LogOff()
        {
            _auth.DeleteAuthCookie();

            return Redirect("/auth/logon");
        }
    }
}
