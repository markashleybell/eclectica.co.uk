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
    public class LinkRepository : RepositoryBase<Link>, ILinkRepository
    {
        public LinkRepository(IConnectionFactory connectionFactory) : base(connectionFactory) { }

        public override IEnumerable<Link> All()
        {
            IEnumerable<Link> tags;

            using (var conn = base.GetOpenConnection())
            {
                using (_profiler.Step("Get all links"))
                {
                    tags = conn.Query<Link>("SELECT l.* FROM Links AS l");
                }
            }

            return tags;
        }

        public override Link Get(long id)
        {
            var sql = "SELECT l.* " +
                      "FROM Links AS l " +
                      "WHERE l.LinkID = @LinkID";

            Link link;

            using (var conn = base.GetOpenConnection())
            {
                using (_profiler.Step("Get link by id (" + id + ")"))
                {
                    // Get the entry details
                    link = conn.Query<Link>(sql, new { LinkID = id }).FirstOrDefault();
                }
            }

            return link;
        }

        public override void Add(Link link)
        {
            var sql = "INSERT INTO Links (Title, Url, Category) " +
                      "VALUES (@Title, @Url, @Category)";

            var newId = 0;

            using (var conn = base.GetOpenConnection())
            {
                using (_profiler.Step("Add link"))
                {
                    // We need to get the new ID with a separate query inside this transaction, because
                    // we want to support SQL CE, which doesn't support batch queries or SCOPE_IDENTITY()
                    using (var transaction = conn.BeginTransaction())
                    {
                        // Do the insert and retrieve the new ID
                        conn.Execute(sql, link, transaction);
                        newId = (int)conn.Query<decimal>("SELECT @@IDENTITY", null, transaction).First();

                        transaction.Commit();
                    }
                }
            }

            link.LinkID = newId;
        }

        public override void Update(Link link)
        {
            var sql = "UPDATE Links " +
                      "SET Title = @Title, Url = @Url, Category = @Category " +
                      "WHERE LinkID = @LinkID";

            using (var conn = base.GetOpenConnection())
            {
                using (_profiler.Step("Update link"))
                {
                    // Get the entry details
                    conn.Execute(sql, link);
                }
            }
        }

        public override void Remove(long id)
        {
            using (var conn = base.GetOpenConnection())
            {
                using (_profiler.Step("Delete link"))
                {
                    conn.Execute("DELETE FROM Links WHERE LinkID = @LinkID", new { LinkID = id });
                }
            }
        }        
    }
}
