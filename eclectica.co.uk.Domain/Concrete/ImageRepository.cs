using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Domain.Abstract;
using eclectica.co.uk.Domain.Entities;

namespace eclectica.co.uk.Domain.Concrete
{
    public class ImageRepository : RepositoryBase<Image>, IImageRepository
    {
        public ImageRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public Image Get(int id)
        {
            return base.Get(id);
        }
    }
}
