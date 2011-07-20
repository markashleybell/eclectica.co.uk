using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Domain.Abstract;
using eclectica.co.uk.Domain.Entities;
using System.Data.Entity;
using System.Linq.Expressions;
using MvcMiniProfiler;

namespace eclectica.co.uk.Domain.Concrete
{
    public class EntryRepository : RepositoryBase<Entry>, IEntryRepository
    {
        public EntryRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
        
        private MiniProfiler profiler = MiniProfiler.Current;

        public Entry GetByUrl(string url)
        {
            using (profiler.Step("Get Entry from Repo"))
            {
                return base.Query(x => x.Url == url).FirstOrDefault();
            }
        }
    }
}
