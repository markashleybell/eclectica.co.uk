using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Domain.Abstract;
using eclectica.co.uk.Domain.Entities;
using System.Data.Entity;

namespace eclectica.co.uk.Domain.Concrete
{
    public class EntryRepository : RepositoryBase<Entry>, IEntryRepository
    {
        public EntryRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public override IEnumerable<Entry> All()
        {
            return base.All().AsQueryable().Include("Tags").Include("Author");
        }

        public override Entry Get(long id)
        {
            return base.Query(x => x.EntryID == id).FirstOrDefault();
        }

        public Entry GetByUrl(string url)
        {
            return base.Query(x => x.Url == url).FirstOrDefault();
        }
    }
}
