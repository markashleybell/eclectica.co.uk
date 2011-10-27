using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Domain.Abstract;
using eclectica.co.uk.Domain.Entities;
using System.Linq.Expressions;
using System.Data;
using Dapper;
using MvcMiniProfiler;

namespace eclectica.co.uk.Domain.Concrete
{
    public class EntryRepository : RepositoryBase<Entry>, IEntryRepository
    {
        private MiniProfiler _profiler = MiniProfiler.Current;

        public EntryRepository(IDbConnection connection) : base(connection) { }

        public Entry GetByUrl(string url)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Entry> All()
        {
            IEnumerable<Entry> entries;

            using (base.Connection)
            {
                base.Connection.Open();
                entries = base.Connection.Query<Entry>("select Title, Url, Published, Updated, Body from Entries");
            }

            return entries;
        }

        public IEnumerable<Entry> Page(int start, int count)
        {
            var sql = "SELECT e.*, c.CommentCount, a.* " +
                      "FROM Entries AS e " +
                      "LEFT OUTER JOIN Authors AS a ON a.AuthorID = e.Author_AuthorID " +
                      "LEFT OUTER JOIN (SELECT Entry_EntryID, COUNT(*) as CommentCount " +
                      "FROM Comments GROUP BY Entry_EntryID) AS c " +
                      "ON e.EntryID = c.Entry_EntryID " +
                      "ORDER BY e.Published DESC OFFSET @Offset ROWS FETCH NEXT @Count ROWS ONLY";

            var tagSql = "SELECT t.* " +
                         "FROM Tags AS t " + 
                         "INNER JOIN EntryTags AS et ON et.Tag_TagID = t.TagID " +
                         "WHERE et.Entry_EntryID = @EntryID";

            IEnumerable<Entry> entries;
            IEnumerable<Tag> tags;

            using(_profiler.Step("Get entries for page"))
            {
                using(base.Connection)
                {
                    base.Connection.Open();

                    // Get the entries for this page
                    entries = base.Connection.Query<Entry, Author, Entry>(sql, (entry, author) => {
                        entry.Author = author;
                        entry.Tags = base.Connection.Query<Tag>(tagSql, new {
                            EntryID = entry.EntryID
                        }).ToList();
                        return entry;
                    }, new {
                        Offset = start,
                        Count = count
                    }, splitOn: "AuthorID");
                }
            }

            return entries;
        }

        public override Entry Get(long id)
        {
            throw new NotImplementedException();
        }

        public override void Add(Entry entity)
        {
            throw new NotImplementedException();
        }

        public override void Remove(long id)
        {
            throw new NotImplementedException();
        }
    }
}
