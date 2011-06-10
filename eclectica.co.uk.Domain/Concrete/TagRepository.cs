using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Domain.Abstract;
using eclectica.co.uk.Domain.Entities;

namespace eclectica.co.uk.Domain.Concrete
{
    public class TagRepository : RepositoryBase<Tag>, ITagRepository
    {
        public TagRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public Tag Get(int id)
        {
            return base.Get(id);
        }
    }
}
