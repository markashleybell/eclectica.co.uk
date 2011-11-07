using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Domain.Abstract;
using eclectica.co.uk.Domain.Entities;

namespace eclectica.co.uk.Test.Data
{
    public class FakeCommentRepository : ICommentRepository
    {
        public IEnumerable<Comment> All()
        {
            throw new NotImplementedException();
        }

        public Comment Get(long id)
        {
            throw new NotImplementedException();
        }

        public void Add(Comment entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Comment entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(long id)
        {
            throw new NotImplementedException();
        }
    }
}
