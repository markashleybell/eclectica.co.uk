using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Domain.Abstract;
using eclectica.co.uk.Domain.Entities;

namespace eclectica.co.uk.Test.Data
{
    public class FakeAuthorRepository : IAuthorRepository
    {
        public IEnumerable<Author> All()
        {
            throw new NotImplementedException();
        }

        public Author Get(long id)
        {
            throw new NotImplementedException();
        }

        public void Add(Author entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Author entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(long id)
        {
            throw new NotImplementedException();
        }
    }
}
