using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Service.Entities;
using eclectica.co.uk.Domain.Entities;

namespace eclectica.co.uk.Service.Abstract
{
    public interface IRedirectServices
    {
        IEnumerable<RedirectModel> All();

        RedirectModel GetRedirect(int id);

        void AddRedirect(RedirectModel model);
        void UpdateRedirect(RedirectModel model);
        void DeleteRedirect(int id);
    }
}
