using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eclectica.co.uk.Service.Abstract;
using eclectica.co.uk.Web.Abstract;
using System.Net;

namespace eclectica.co.uk.Web.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult ServerError()
        {
            return View();
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult OtherHttpStatusCode()
        {
            return View();
        }

        // This action allows us to test custom error handling and logging
        public ActionResult ErrorTest(int code)
        {
            switch (code)
            {
                case 404:
                    throw new HttpException((int)HttpStatusCode.NotFound, "Page not found");
                case 500:
                    throw new HttpException((int)HttpStatusCode.InternalServerError, "Internal server error");
            }

            return Content("");
        }
    }
}
