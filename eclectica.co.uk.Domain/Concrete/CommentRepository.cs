using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Domain.Abstract;
using eclectica.co.uk.Domain.Entities;
using System.Data;

namespace eclectica.co.uk.Domain.Concrete
{
    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        public CommentRepository(IDbConnection connection) : base(connection) { }

        public override IEnumerable<Comment> All()
        {
            throw new NotImplementedException();
        }

        public override Comment Get(long id)
        {
            throw new NotImplementedException();
        }

        public override void Add(Comment entity)
        {
            throw new NotImplementedException();
        }

        public override void Remove(long id)
        {
            throw new NotImplementedException();
        }
    }
}
