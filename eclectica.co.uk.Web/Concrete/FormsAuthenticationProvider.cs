using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using eclectica.co.uk.Web.Abstract;

namespace eclectica.co.uk.Web.Concrete
{
    public class FormsAuthenticationProvider : IFormsAuthenticationProvider
    {
        public bool Authenticate(string userName, string password)
        {
            return FormsAuthentication.Authenticate(userName, password);
        }

        public void SetAuthCookie(string userName, bool persistent)
        {
            FormsAuthentication.SetAuthCookie(userName, persistent);
        }

        public void DeleteAuthCookie()
        {
            FormsAuthentication.SignOut();
        }
    }
}