using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Domain.Abstract;
using eclectica.co.uk.Domain.Entities;
using System.Data;

namespace eclectica.co.uk.Domain.Concrete
{
    public class LinkRepository : RepositoryBase<Link>, ILinkRepository
    {
        public LinkRepository(IDbConnection connection) : base(connection) { }

        public override IEnumerable<Link> All()
        {
            throw new NotImplementedException();
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
