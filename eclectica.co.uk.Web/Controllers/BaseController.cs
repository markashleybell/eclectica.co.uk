using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eclectica.co.uk.Service.Abstract;
using eclectica.co.uk.Web.Abstract;

namespace eclectica.co.uk.Web.Controllers
{
    public class BaseController : Controller
    {
        protected IEntryServices _entryServices;
        protected ICommentServices _commentServices;
        protected IConfigurationInfo _config;

        public BaseController(IEntryServices entryServices, ICommentServices commentServices, IConfigurationInfo config)
        {
            _entryServices = entryServices;
            _commentServices = commentServices;
            _config = config;
        }
    }
}
