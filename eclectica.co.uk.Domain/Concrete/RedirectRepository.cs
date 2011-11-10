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
    public class RedirectRepository : RepositoryBase<Redirect>, IRedirectRepository
    {
        public RedirectRepository(IConnectionFactory connectionFactory) : base(connectionFactory) { }

        public override IEnumerable<Redirect> All()
        {
            IEnumerable<Redirect> tags;

            using (var conn = base.GetOpenConnection())
            {
                using (_profiler.Step("Get all redirects"))
                {
                    tags = conn.Query<Redirect>("SELECT r.* FROM Redirects AS r ORDER BY r.RedirectID DESC");
                }
            }

            return tags;
        }

        public override Redirect Get(long id)
        {
            var sql = "SELECT r.* " +
                      "FROM Redirects AS r " +
                      "WHERE r.RedirectID = @RedirectID";

            Redirect redirect;

            using (var conn = base.GetOpenConnection())
            {
                using (_profiler.Step("Get redirect by id (" + id + ")"))
                {
                    // Get the entry details
                    redirect = conn.Query<Redirect>(sql, new { RedirectID = id }).FirstOrDefault();
                }
            }

            return redirect;
        }

        public override void Add(Redirect redirect)
        {
            var sql = "INSERT INTO Redirects (RedirectUrl, Clicks) " +
                      "VALUES (@RedirectUrl, 0)";

            var newId = 0;

            using (var conn = base.GetOpenConnection())
            {
                using (_profiler.Step("Add redirect"))
                {
                    // We need to get the new ID with a separate query inside this transaction, because
                    // we want to support SQL CE, which doesn't support batch queries or SCOPE_IDENTITY()
                    using (var transaction = conn.BeginTransaction())
                    {
                        // Do the insert and retrieve the new ID
                        conn.Execute(sql, redirect, transaction);
                        newId = (int)conn.Query<decimal>("SELECT @@IDENTITY", null, transaction).First();

                        transaction.Commit();
                    }
                }
            }

            redirect.RedirectID = newId;
        }

        public override void Update(Redirect redirect)
        {
            var sql = "UPDATE Redirects " +
                      "SET RedirectUrl = @RedirectUrl, Clicks = @Clicks " +
                      "WHERE RedirectID = @RedirectID";

            using (var conn = base.GetOpenConnection())
            {
                using (_profiler.Step("Update redirect"))
                {
                    // Get the entry details
                    conn.Execute(sql, redirect);
                }
            }
        }

        public override void Remove(long id)
        {
            using (var conn = base.GetOpenConnection())
            {
                using (_profiler.Step("Delete redirect"))
                {
                    conn.Execute("DELETE FROM Redirects WHERE RedirectID = @RedirectID", new { RedirectID = id });
                }
            }
        }        
    }
}
