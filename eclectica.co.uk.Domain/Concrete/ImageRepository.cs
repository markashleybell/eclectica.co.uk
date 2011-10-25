using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Domain.Abstract;
using eclectica.co.uk.Domain.Entities;
using System.Data;

namespace eclectica.co.uk.Domain.Concrete
{
    public class ImageRepository : RepositoryBase<Image>, IImageRepository
    {
        public ImageRepository(IDbConnection connection) : base(connection) { }

        public override IEnumerable<Image> All()
        {
            throw new NotImplementedException();
        }

        public override Image Get(long id)
        {
            throw new NotImplementedException();
        }

        public override void Add(Image entity)
        {
            throw new NotImplementedException();
        }

        public override void Remove(long id)
        {
            throw new NotImplementedException();
        }
    }
}
