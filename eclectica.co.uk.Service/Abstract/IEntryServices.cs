using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Service.Entities;

namespace eclectica.co.uk.Service.Abstract
{
    public interface IEntryServices
    {
        IEnumerable<EntryModel> All();
        EntryModel GetEntry(int id);
        EntryModel GetEntryByUrl(string folder);

        IEnumerable<EntryModel> Last(int count);
    }
}
