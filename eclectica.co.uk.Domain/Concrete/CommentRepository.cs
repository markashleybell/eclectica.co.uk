﻿using System;
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

        public override void Add(Comment entity)
        {
            throw new NotImplementedException();
        }

        public override void Update(Comment entity)
        {
            throw new NotImplementedException();
        }

        public override void Remove(long id)
        {
            throw new NotImplementedException();
        }
    }
}
