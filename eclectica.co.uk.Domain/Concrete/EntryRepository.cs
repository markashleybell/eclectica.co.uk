using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Domain.Abstract;
using eclectica.co.uk.Domain.Entities;
using System.Data.Entity;
using System.Linq.Expressions;

namespace eclectica.co.uk.Domain.Concrete
{
    public class EntryRepository : RepositoryBase<Entry>, IEntryRepository
    {
        public EntryRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public Entry GetByUrl(string url)
        {
            return base.Query(x => x.Url == url).FirstOrDefault();
        }
    }
}
