using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Domain.Abstract;
using eclectica.co.uk.Domain.Entities;

namespace eclectica.co.uk.Domain.Concrete
{
    public class LinkRepository : RepositoryBase<Link>, ILinkRepository
    {
        public LinkRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public Link Get(int id)
        {
            return base.Get(id);
        }
    }
}
