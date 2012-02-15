using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eclectica.co.uk.Service.Abstract;
using eclectica.co.uk.Web.Abstract;
using System.Net;
using System.Configuration;
using Elmah;
using System.Security.Cryptography;
using bcrypt = BCrypt.Net.BCrypt;

namespace eclectica.co.uk.Web.Controllers
{
    public class ErrorController : Controller
    {
        [Authorize]
        public ActionResult ErrorSummary()
        {
            var errorList = new List<ErrorLogEntry>();
            var log = ErrorLog.GetDefault(System.Web.HttpContext.Current);

            log.GetErrors(0, 1000, errorList);

            var errors = new List<ErrorLogEntry>();

            foreach (var entry in errorList)
                errors.Add(log.GetError(entry.Id));

            return View(errors);
        }

        public ActionResult ErrorDigest(string key)
        {
            var hash = ConfigurationManager.AppSettings["ErrorDigestKey"];

            if (!bcrypt.Verify(key, hash))
            {
                throw new HttpException((int)HttpStatusCode.NotFound, ConfigurationManager.AppSettings["Error404Message"]);
            }
            else
            {
                var errorList = new List<ErrorLogEntry>();
                var log = ErrorLog.GetDefault(System.Web.HttpContext.Current);

                log.GetErrors(0, 1000, errorList);

                var errors = new List<ErrorLogEntry>();

                foreach (var entry in errorList)
                    errors.Add(log.GetError(entry.Id));

                //return Content(errors.Count.ToString());
                return View(errors);
            }
        }

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
            // Access config properties directly here, to avoid error controller depending on Ninject DI
            switch (code)
            {
                case 403:
                    throw new HttpException((int)HttpStatusCode.Forbidden, ConfigurationManager.AppSettings["Error403Message"]);
                case 404:
                    throw new HttpException((int)HttpStatusCode.NotFound, ConfigurationManager.AppSettings["Error404Message"]);
                case 500:
                    throw new HttpException((int)HttpStatusCode.InternalServerError, ConfigurationManager.AppSettings["Error500Message"]);
            }

            return Content("");
        }
    }
}
