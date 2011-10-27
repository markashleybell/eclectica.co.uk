using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Domain.Abstract;
using eclectica.co.uk.Domain.Entities;
using System.Data;

namespace eclectica.co.uk.Domain.Concrete
{
    public class TagRepository : RepositoryBase<Tag>, ITagRepository
    {
        public TagRepository(IConnectionFactory connectionFactory) : base(connectionFactory) { }

        public override IEnumerable<Tag> All()
        {
            throw new NotImplementedException();
        }

        public override Tag Get(long id)
        {
            throw new NotImplementedException();
        }

        public override void Add(Tag entity)
        {
            throw new NotImplementedException();
        }

        public override void Remove(long id)
        {
            throw new NotImplementedException();
        }
    }
}
