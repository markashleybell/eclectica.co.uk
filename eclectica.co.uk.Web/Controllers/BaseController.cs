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
        protected ITagServices _tagServices;
        protected ILinkServices _linkServices;

        public BaseController(IEntryServices entryServices, ICommentServices commentServices, ITagServices tagServices, ILinkServices linkServices)
        {
            _entryServices = entryServices;
            _commentServices = commentServices;
            _tagServices = tagServices;
            _linkServices = linkServices;
        }
    }
}
