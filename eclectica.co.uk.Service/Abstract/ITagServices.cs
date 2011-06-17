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

        Dictionary<string, List<TagModel>> GetSortedTags();

        Dictionary<string, List<EntryModel>> GetEntriesForTag(string tagName);
    }
}
