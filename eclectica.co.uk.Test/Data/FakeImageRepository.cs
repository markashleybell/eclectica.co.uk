using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Domain.Abstract;
using eclectica.co.uk.Domain.Entities;

namespace eclectica.co.uk.Test.Data
{
    public class FakeImageRepository : IImageRepository
    {
        public IEnumerable<Image> Last25()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Image> All()
        {
            throw new NotImplementedException();
        }

        public Image Get(long id)
        {
            throw new NotImplementedException();
        }

        public void Add(Image entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Image entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(long id)
        {
            throw new NotImplementedException();
        }
    }
}
