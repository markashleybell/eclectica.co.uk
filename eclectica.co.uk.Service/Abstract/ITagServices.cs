using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Service.Entities;

namespace eclectica.co.uk.Service.Abstract
{
    public interface ITagServices
    {
        IEnumerable<TagModel> All();
        IEnumerable<TagModel> Search(string query);

        Dictionary<string, List<TagModel>> GetSortedTags();
    }
}
