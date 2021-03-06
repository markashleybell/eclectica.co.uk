﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eclectica.co.uk.Web.Abstract
{
    public interface IFormsAuthenticationProvider
    {
        bool Authenticate(string userName, string password);
        void SetAuthCookie(string userName, bool persistent);
        void DeleteAuthCookie();
    }
}
