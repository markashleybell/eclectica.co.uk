using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Domain.Abstract;
using eclectica.co.uk.Domain.Entities;
using System.Data;
using MvcMiniProfiler;
using Dapper;

namespace eclectica.co.uk.Domain.Concrete
{
    public class ImageRepository : RepositoryBase<Image>, IImageRepository
    {
        public ImageRepository(IConnectionFactory connectionFactory) : base(connectionFactory) { }

        public override IEnumerable<Image> All()
        {
            IEnumerable<Image> images;

            using (var conn = base.GetOpenConnection())
            {
                using (_profiler.Step("Get all images"))
                {
                    images = conn.Query<Image>("SELECT i.* FROM Images AS i ORDER BY i.ImageID DESC").ToList();
                }
            }

            return images;
        }

        public override Image Get(long id)
        {
            throw new NotImplementedException();
        }

        public override void Add(Image image)
        {
            var sql = "INSERT INTO Images (Filename) VALUES (@Filename)";

            var newId = 0;

            using (var conn = base.GetOpenConnection())
            {
                using (_profiler.Step("Add image"))
                {
                    // We need to get the new ID with a separate query inside this transaction, because
                    // we want to support SQL CE, which doesn't support batch queries or SCOPE_IDENTITY()
                    using (var transaction = conn.BeginTransaction())
                    {
                        // Do the insert and retrieve the new ID
                        conn.Execute(sql, image, transaction);
                        newId = (int)conn.Query<decimal>("SELECT @@IDENTITY", null, transaction).First();

                        transaction.Commit();
                    }
                }
            }

            image.ImageID = newId;
        }

        public override void Update(Image entity)
        {
            throw new NotImplementedException();
        }

        public override void Remove(long id)
        {
            throw new NotImplementedException();
        }
    }
}
