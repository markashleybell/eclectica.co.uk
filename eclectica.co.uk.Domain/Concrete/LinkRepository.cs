using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Domain.Abstract;
using eclectica.co.uk.Domain.Entities;
using System.Data;
using MvcMiniProfiler;
using Dapper;

namespace eclectica.co.uk.Domain.Concrete
{
    public class LinkRepository : RepositoryBase<Link>, ILinkRepository
    {
        public LinkRepository(IConnectionFactory connectionFactory) : base(connectionFactory) { }

        public override IEnumerable<Link> All()
        {
            IEnumerable<Link> tags;

            using (var conn = base.GetOpenConnection())
            {
                using (_profiler.Step("Get all links"))
                {
                    tags = conn.Query<Link>("SELECT l.* FROM Links AS l");
                }
            }

            return tags;
        }

        public override Link Get(long id)
        {
            throw new NotImplementedException();
        }

        public override void Add(Link entity)
        {
            throw new NotImplementedException();
        }

        public override void Remove(long id)
        {
            throw new NotImplementedException();
        }
    }
}
