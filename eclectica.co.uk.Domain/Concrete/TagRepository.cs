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
    public class TagRepository : RepositoryBase<Tag>, ITagRepository
    {
        public TagRepository(IConnectionFactory connectionFactory) : base(connectionFactory) { }

        public override IEnumerable<Tag> All()
        {
            var sql = "SELECT t.TagName, COUNT(t.TagID) AS UseCount " +
                      "FROM Tags AS t " +
                      "INNER JOIN EntryTags AS et ON et.Tag_TagID = t.TagID " +
                      "GROUP BY t.TagName " +
                      "ORDER BY t.TagName";

            IEnumerable<Tag> tags;

            using (var conn = base.GetOpenConnection())
            {
                using (_profiler.Step("Get all tags and use counts"))
                {
                    tags = conn.Query<Tag>(sql);
                }
            }

            return tags;
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
