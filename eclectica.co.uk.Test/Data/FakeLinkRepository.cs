using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Domain.Abstract;
using eclectica.co.uk.Domain.Entities;

namespace eclectica.co.uk.Test.Data
{
    public class FakeLinkRepository : ILinkRepository
    {
        public IEnumerable<Link> All()
        {
            throw new NotImplementedException();
        }

        public Link Get(long id)
        {
            throw new NotImplementedException();
        }

        public void Add(Link entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Link entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(long id)
        {
            throw new NotImplementedException();
        }
    }
}
