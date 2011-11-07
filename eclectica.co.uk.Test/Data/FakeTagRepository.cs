using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Domain.Abstract;
using eclectica.co.uk.Domain.Entities;

namespace eclectica.co.uk.Test.Data
{
    public class FakeTagRepository : ITagRepository
    {
        public IEnumerable<Tag> Like(string query)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tag> All()
        {
            throw new NotImplementedException();
        }

        public Tag Get(long id)
        {
            throw new NotImplementedException();
        }

        public void Add(Tag entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Tag entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(long id)
        {
            throw new NotImplementedException();
        }
    }
}
