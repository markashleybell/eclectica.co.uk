using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Domain.Abstract;
using eclectica.co.uk.Domain.Entities;
using System.Data;

namespace eclectica.co.uk.Domain.Concrete
{
    public class AuthorRepository : RepositoryBase<Author>, IAuthorRepository
    {
        public AuthorRepository(IConnectionFactory connectionFactory) : base(connectionFactory) { }

        public override IEnumerable<Author> All()
        {
            throw new NotImplementedException();
        }

        public override Author Get(long id)
        {
            throw new NotImplementedException();
        }

        public override void Add(Author entity)
        {
            throw new NotImplementedException();
        }

        public override void Update(Author entity)
        {
            throw new NotImplementedException();
        }

        public override void Remove(long id)
        {
            throw new NotImplementedException();
        }
    }
}
