using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Domain.Abstract;
using eclectica.co.uk.Domain.Entities;

namespace eclectica.co.uk.Domain.Concrete
{
    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        public CommentRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public Comment Get(int id)
        {
            return base.Get(id);
        }
    }
}
