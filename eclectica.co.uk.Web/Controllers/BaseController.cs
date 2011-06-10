using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eclectica.co.uk.Service.Abstract;

namespace eclectica.co.uk.Web.Controllers
{
    public class BaseController : Controller
    {
        protected IEntryServices _entryServices;
        protected ICommentServices _commentServices;

        public BaseController(IEntryServices entryServices, ICommentServices commentServices)
        {
            _entryServices = entryServices;
            _commentServices = commentServices;
        }
    }
}
