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

            var tagSql = "SELECT t.TagID, t.TagName, et.Entry_EntryID as EntryID " +
                         "FROM Tags AS t " + 
                         "INNER JOIN EntryTags AS et ON et.Tag_TagID = t.TagID " +
                         "WHERE et.Entry_EntryID IN @EntryIDs";

            IEnumerable<Entry> entries;
            IEnumerable<dynamic> tags;

            using(base.Connection)
            {
                base.Connection.Open();

                using(_profiler.Step("Get entries for page"))
                {
                    // Get the entries for this page
                    entries = base.Connection.Query<Entry, Author, Entry>(sql, (entry, author) => {
                        entry.Author = author;
                        return entry;
                    }, new {
                        Offset = start,
                        Count = count
                    }, splitOn: "AuthorID");
                }

                using(_profiler.Step("Get tags"))
                {
                    // Get the tags for this page
                    tags = base.Connection.Query(tagSql, new { EntryIDs = entries.Select(e => e.EntryID).ToArray() });
                }
            }

            // Map the anonymous tag objects to proper entities and map to the correct entries
            return entries.Select(e => {
                e.Tags = (from t in tags.Where(t => t.EntryID == e.EntryID)
                          select new Tag {
                              TagID = t.TagID,
                              TagName = t.TagName
                          }).ToList();

                return e;
            });
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
