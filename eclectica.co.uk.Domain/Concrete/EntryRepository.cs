using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Domain.Abstract;
using eclectica.co.uk.Domain.Entities;
using System.Linq.Expressions;
using System.Data;
using Dapper;

namespace eclectica.co.uk.Domain.Concrete
{
    public class EntryRepository : RepositoryBase<Entry>, IEntryRepository
    {
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
            IEnumerable<Entry> entries;

            //var sql = "SELECT e.EntryID, e.Title, e.Url, e.Published, e.Updated, CAST(e.Body AS nvarchar(512)) as e.Body, e.Tweet, e.Publish, e.Author_AuthorID, COUNT(c.CommentID) AS CommentCount, a.AuthorID, a.Name, a.Email " +
            //          "FROM Entries AS e " +
            //          "LEFT OUTER JOIN Comments AS c ON c.Entry_EntryID = e.EntryID " +
            //          "LEFT OUTER JOIN Authors AS a ON a.AuthorID = e.Author_AuthorID " +
            //          "GROUP BY e.EntryID, e.Title, e.Url, e.Published, e.Updated, CAST(e.Body AS nvarchar(512)), e.Tweet, e.Publish, e.Author_AuthorID, a.AuthorID, a.Name, a.Email " + 
            //          "order by Published desc offset @Offset rows fetch next @Count rows only;";

            var sql = "SELECT e.*, c.CommentCount, a.* " +
                      "FROM Entries AS e " +
                      "LEFT OUTER JOIN Authors AS a ON a.AuthorID = e.Author_AuthorID " +
                      "LEFT OUTER JOIN (SELECT Entry_EntryID, COUNT(*) as CommentCount " +
                      "FROM Comments GROUP BY Entry_EntryID) AS c " +
                      "ON e.EntryID = c.Entry_EntryID " +
                      "ORDER BY e.Published DESC OFFSET @Offset ROWS FETCH NEXT @Count ROWS ONLY";

            using (base.Connection)
            {
                base.Connection.Open();
                entries = base.Connection.Query<Entry, Author, Entry>(sql, (entry, author) => { 
                    entry.Author = author;
                    return entry; 
                }, new { 
                    Offset = start, 
                    Count = count
                }, splitOn: "AuthorID");
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
