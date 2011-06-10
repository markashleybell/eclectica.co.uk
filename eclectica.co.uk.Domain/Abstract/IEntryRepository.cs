using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Domain.Entities;

namespace eclectica.co.uk.Domain.Abstract
{
    public interface IEntryRepository : IRepository<Entry>
    {
        Entry GetByUrl(string url);
    }
}
