using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Service.Entities;
using eclectica.co.uk.Domain.Entities;

namespace eclectica.co.uk.Service.Abstract
{
    public interface ILinkServices
    {
        IEnumerable<LinkModel> All();

        LinkModel GetLink(int id);

        IEnumerable<LinkModel> GetSortedLinks();

        void AddLink(LinkModel model);
        void UpdateLink(LinkModel model);
        void DeleteLink(int id);
    }
}
