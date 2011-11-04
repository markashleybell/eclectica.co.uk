using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Domain.Abstract;
using eclectica.co.uk.Domain.Entities;
using System.Data;
using Dapper;
using MvcMiniProfiler;

namespace eclectica.co.uk.Domain.Concrete
{
    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        public CommentRepository(IConnectionFactory connectionFactory) : base(connectionFactory) { }

        public override IEnumerable<Comment> All()
        {
            IEnumerable<Comment> comments;

            using (var conn = base.GetOpenConnection())
            {
                using (_profiler.Step("Get all comments"))
                {
                    comments = conn.Query<Comment>("SELECT c.* FROM Comments AS c ORDER BY c.CommentID DESC").ToList();
                }
            }

            return comments;
        }

        public override Comment Get(long id)
        {
            var sql = "SELECT c.* " +
                      "FROM Comments AS c " +
                      "WHERE c.CommentID = @CommentID";

            Comment comment;

            using (var conn = base.GetOpenConnection())
            {
                using (_profiler.Step("Get comment by id (" + id + ")"))
                {
                    // Get the entry details
                    comment = conn.Query<Comment>(sql, new { CommentID = id }).FirstOrDefault();
                }
            }

            return comment;
        }

        public override void Add(Comment comment)
        {
            var sql = "INSERT INTO Comments (Name, Email, Url, Body, RawBody, Date, Approved, Entry_EntryID) " +
                      "VALUES (@Name, @Email, @Url, @Body, @RawBody, @Date, 1, @EntryID)";

            comment.Date = DateTime.Now;

            var newId = 0;

            using (var conn = base.GetOpenConnection())
            {
                using (_profiler.Step("Add comment"))
                {
                    // We need to get the new ID with a separate query inside this transaction, because
                    // we want to support SQL CE, which doesn't support batch queries or SCOPE_IDENTITY()
                    using (var transaction = conn.BeginTransaction())
                    {
                        // Do the insert and retrieve the new ID
                        conn.Execute(sql, comment, transaction);
                        newId = (int)conn.Query<decimal>("SELECT @@IDENTITY", null, transaction).First();

                        transaction.Commit();
                    }
                }
            }

            comment.CommentID = newId;
        }

        public override void Update(Comment comment)
        {
            var sql = "UPDATE Comments " +
                      "SET Name = @Name, Email = @Email, Url = @Url, Body = @Body, RawBody = @RawBody, Date = @Date, Approved = @Approved " +
                      "WHERE CommentID = @CommentID";

            comment.Date = DateTime.Now;

            using (var conn = base.GetOpenConnection())
            {
                using (_profiler.Step("Update comment"))
                {
                    // Get the entry details
                    conn.Execute(sql, comment);
                }
            }
        }

        public override void Remove(long id)
        {
            using (var conn = base.GetOpenConnection())
            {
                using (_profiler.Step("Delete comment"))
                {
                    conn.Execute("DELETE FROM Comments WHERE CommentID = @CommentID", new { CommentID = id });
                }
            }
        }
    }
}
