using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Domain.Abstract;
using eclectica.co.uk.Domain.Entities;
using eclectica.co.uk.Domain.Extensions;
using System.Linq.Expressions;
using System.Data;
using System.Data.Common;
using Dapper;
using MvcMiniProfiler;
using System.Text.RegularExpressions;
using System.Collections;
using MvcMiniProfiler.Data;
using System.Transactions;

namespace eclectica.co.uk.Domain.Concrete
{
    public class EntryRepository : RepositoryBase<Entry>, IEntryRepository
    {
        private IELMAHConnectionFactory _elmahConnectionFactory;

        public EntryRepository(IConnectionFactory connectionFactory, IELMAHConnectionFactory elmahConnectionFactory) : base(connectionFactory) 
        {
            _elmahConnectionFactory = elmahConnectionFactory;
        }

        public Entry GetByUrl(string url)
        {
            var sql = "SELECT e.*, c.CommentCount, a.* " +
                      "FROM Entries AS e " +
                      "LEFT OUTER JOIN Authors AS a ON a.AuthorID = e.Author_AuthorID " +
                      "LEFT OUTER JOIN (SELECT Entry_EntryID, COUNT(*) as CommentCount " +
                      "FROM Comments GROUP BY Entry_EntryID) AS c " +
                      "ON e.EntryID = c.Entry_EntryID " +
                      "WHERE e.Url = @Url AND e.Publish = 1";

            var tagSql = "SELECT t.TagID, t.TagName " +
                         "FROM Tags AS t " + 
                         "INNER JOIN EntryTags AS et ON et.Tag_TagID = t.TagID " +
                         "WHERE et.Entry_EntryID = @EntryID";

            var commentSql = "SELECT c.* " +
                             "FROM Comments AS c " + 
                             "WHERE c.Entry_EntryID = @EntryID AND c.Approved = 1";

            var relatedSql = "SELECT e.EntryID, e.Title, e.Url, e.Body, e.Published " +
                             "FROM Entries AS e " +
                             "INNER JOIN EntryEntries AS ee ON ee.Entry_EntryID1 = e.EntryID " +
                             "WHERE ee.Entry_EntryID = @EntryID AND e.Publish = 1";

            Entry entry;

            using(var conn = base.GetOpenConnection())
            {
                using(_profiler.Step("Get entry by url"))
                {
                    // Get the entry details
                    entry = conn.Query<Entry, Author, Entry>(sql, (e, a) => {
                        e.Author = a;
                        return e;
                    }, new {
                        Url = url,
                    }, splitOn: "AuthorID").FirstOrDefault();
                }

                // If the entry exists
                if (entry != null)
                {
                    // Perform queries for the tags, comments and related entries
                    using (_profiler.Step("Get tags for entry")) { entry.Tags = conn.Query<Tag>(tagSql, new { EntryID = entry.EntryID }).ToList(); }
                    using (_profiler.Step("Get comments for entry")) { entry.Comments = conn.Query<Comment>(commentSql, new { EntryID = entry.EntryID }).ToList(); }
                    using (_profiler.Step("Get related entries for entry"))
                    {
                        entry.Related = conn.Query<Entry>(relatedSql, new { EntryID = entry.EntryID })
                                            .Select(x => {
                                                x.Thumbnail = GetThumbnail(x.Title, x.Body);
                                                return x;
                                            }).ToList();
                    }
                }
            }

            return entry;
        }

        public string[] GetEntryUrls()
        {
            string[] allUrls = null;

            using(var conn = base.GetOpenConnection())
            {
                using(_profiler.Step("Get random entry ID"))
                {
                    // Pull out all the entry urls in the database
                    allUrls = conn.Query<string>("SELECT e.Url from Entries AS e WHERE e.Publish = 1 ORDER BY e.EntryID").ToArray();
                }

            }

            return allUrls;
        }

        private string GetThumbnail(string title, string body)
        {
            if (body == null)
                return "";

            MatchCollection matches = Regex.Matches(body, @"\/img/lib/(.*?)/(.*?)\.(jpg|gif)", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            string thumb = (matches.Count > 0) ? matches[0].Groups[2].Value : "";
            bool drop = (matches.Count > 0 && matches[0].Groups[1].Value == "drop") ? true : false;
            string ext = (matches.Count > 0) ? matches[0].Groups[3].Value : "";
            string bg = (matches.Count > 0) ? "lib/" + ((drop) ? "drop" : "crop") + "/" + thumb + "." + ext : "";

            if(bg == "" && body.Contains("<object"))
                bg = "site/thumb-video.gif";

            if(bg == "")
                bg = "site/" + ((title == "") ? "thumb-quote" : "thumb-article") + ".gif";

            return bg;
        }

        public override IEnumerable<Entry> All()
        {
            var sql = "SELECT e.*, c.CommentCount, a.* " +
                      "FROM Entries AS e " +
                      "LEFT OUTER JOIN Authors AS a ON a.AuthorID = e.Author_AuthorID " +
                      "LEFT OUTER JOIN (SELECT Entry_EntryID, COUNT(*) as CommentCount " +
                      "FROM Comments GROUP BY Entry_EntryID) AS c " +
                      "ON e.EntryID = c.Entry_EntryID " + 
                      "ORDER BY e.Published DESC";

            IEnumerable<Entry> entries;

            using (var conn = base.GetOpenConnection())
            {
                using (_profiler.Step("Get all entries"))
                {

                        // Get the entries for this page
                    entries = conn.Query<Entry, Author, Entry>(sql, (e, a) => {
                        e.Author = a;
                        return e;
                    }, null, splitOn: "AuthorID").Select(x => {
                                      x.Thumbnail = GetThumbnail(x.Title, x.Body);
                                      return x;
                                  }).ToList();
                }
            }

            return entries;
        }

        public IEnumerable<string> UrlList()
        {
            var sql1 = "SELECT e.Url " +
                       "FROM Entries AS e " +
                       "WHERE e.Publish = 1 " +
                       "ORDER BY e.Published DESC";

            var sql2 = "SELECT DISTINCT DATEPART(month, e.Published) AS Month, DATEPART(year, e.Published) AS Year " + 
                       "FROM Entries AS e " +
                       "WHERE e.Publish = 1 " +
                       "ORDER BY Year DESC, Month DESC";

            List<string> urls1;
            List<string> urls2;

            using (var conn = base.GetOpenConnection())
            {
                using (_profiler.Step("Get all published entry urls"))
                {
                    urls1 = conn.Query<string>(sql1).ToList();
                }

                using (_profiler.Step("Get all archive urls"))
                {
                    urls2 = conn.Query(sql2)
                                .Select(x => x.Year + "/" + x.Month)
                                .Cast<string>()
                                .ToList();                    
                }
            }

            return urls1.Concat(urls2).ToList();
        }

        public IEnumerable<Entry> Page(int start, int count)
        {
            var entrySql = "SELECT e.*, c.CommentCount, a.* " +
                           "FROM Entries AS e " +
                           "LEFT OUTER JOIN Authors AS a ON a.AuthorID = e.Author_AuthorID " +
                           "LEFT OUTER JOIN (SELECT Entry_EntryID, COUNT(*) as CommentCount " +
                           "FROM Comments GROUP BY Entry_EntryID) AS c " +
                           "ON e.EntryID = c.Entry_EntryID " +
                           "WHERE e.Publish = 1 " + 
                           "ORDER BY e.Published DESC OFFSET @Offset ROWS FETCH NEXT @Count ROWS ONLY";

            var legacySql = "SELECT  * FROM ( " +
                                "SELECT e.*, c.CommentCount, a.*, ROW_NUMBER() OVER ( ORDER BY e.Published DESC ) AS RowNum " +
                                "FROM Entries AS e " +
                                "LEFT OUTER JOIN Authors AS a ON a.AuthorID = e.Author_AuthorID " +
                                "LEFT OUTER JOIN (SELECT Entry_EntryID, COUNT(*) as CommentCount " +
                                "FROM Comments GROUP BY Entry_EntryID) AS c " +
                                "ON e.EntryID = c.Entry_EntryID " +
                            ") AS RowConstrainedResult " +
                            "WHERE RowNum >= @Offset AND RowNum < (@Offset + @Count) " +
                            "ORDER BY RowNum";

            var tagSql = "SELECT t.TagID, t.TagName, et.Entry_EntryID as EntryID " +
                         "FROM Tags AS t " + 
                         "INNER JOIN EntryTags AS et ON et.Tag_TagID = t.TagID " +
                         "WHERE et.Entry_EntryID IN @EntryIDs";

            IEnumerable<Entry> entries;
            IEnumerable<dynamic> tags;

            // Hack to switch pagination SQL if Database doesn't support OFFSET/FETCH NEXT
            if(base.ServerType == DbServerType.SQL2008)
                entrySql = legacySql;

            using(var conn = base.GetOpenConnection())
            {
                using(_profiler.Step("Get " + count + " entries for current page"))
                {
                    // Get the entries for this page
                    entries = conn.Query<Entry, Author, Entry>(entrySql, (e, a) => {
                        e.Author = a;
                        return e;
                    }, new {
                        Offset = start,
                        Count = count
                    }, splitOn: "AuthorID");
                }

                using(_profiler.Step("Get tags"))
                {
                    // Get the tags for this page
                    tags = conn.Query(tagSql, new { EntryIDs = entries.Select(e => e.EntryID).ToArray() });
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

        public IEnumerable<Entry> Month(int month, int year)
        {
            var sql = "SELECT e.* " +
                      "FROM Entries AS e " +
                      "WHERE e.Publish = 1 AND DATEPART(month, e.Published) = @Month AND DATEPART(year, e.Published) = @Year " + 
                      "ORDER BY e.Published";

            IEnumerable<Entry> entries;

            using (var conn = base.GetOpenConnection())
            {
                using (_profiler.Step("Get entries for month"))
                {
                    // Get the entries for this page
                    entries = conn.Query<Entry>(sql, new { Month = month, Year = year })
                                  .Select(x => {
                                      x.Thumbnail = GetThumbnail(x.Title, x.Body);
                                      return x;
                                  }).ToList();
                }
            }

            return entries;
        }

        public IEnumerable<Entry> GetLatest(int count)
        {
            var sql = "SELECT TOP(@Count) e.Title, e.Url, e.Published " +
                      "FROM Entries AS e " +
                      "WHERE e.Publish = 1 " + 
                      "ORDER BY e.Published DESC";

            IEnumerable<Entry> entries;

            using (var conn = base.GetOpenConnection())
            {
                using (_profiler.Step("Get top " + count + " most recent entries"))
                {
                    // Get the entries for this page
                    entries = conn.Query<Entry>(sql, new { Count = count })
                                  .Select(x => {
                                      x.Title = (!string.IsNullOrEmpty(x.Title)) ? x.Title : x.Published.ToString("dddd dd MMMM yyyy hh:mm");
                                      return x;
                                  }).ToList();
                }
            }

            return entries;
        }

        public IEnumerable<Entry> GetByTag(string tag)
        {
            var sql = "SELECT e.Title, e.Published, e.Body, e.Url " +
                      "FROM Entries AS e " +
                      "INNER JOIN EntryTags AS et ON et.Entry_EntryID = e.EntryID " +
                      "INNER JOIN Tags AS t ON t.TagID = et.Tag_TagID " +
                      "WHERE e.Publish = 1 AND t.TagName = @Tag";

            IEnumerable<Entry> entries;

            using (var conn = base.GetOpenConnection())
            {
                using (_profiler.Step("Get entries for tag '" + tag + "'"))
                {
                    // Get the entries for this tag
                    entries = conn.Query<Entry>(sql, new { Tag = tag });
                }
            }

            return entries;
        }

        public IDictionary<DateTime, int> GetPostCounts(int year)
        {
            var sql = "SELECT DATEPART(month, Published) AS Month, COUNT(EntryID) AS PostCount " +
                      "FROM Entries AS e " +
                      "WHERE Publish = 1 AND DATEPART(year, Published) = @Year " +
                      "GROUP BY DATEPART(month, Published) " +
                      "ORDER BY Month";

            IDictionary<DateTime, int> counts;

            using (var conn = base.GetOpenConnection())
            {
                using(_profiler.Step("Get post counts"))
                {
                    counts = (from m in conn.Query(sql, new { Year = year })
                              select new { Month = m.Month, Count = m.PostCount }).ToList().ToDictionary(x => new DateTime(year, x.Month, 1), x => (int)x.Count);
                }
            }

            return counts;
        }

        public override Entry Get(long id)
        {
            // This is the ONLY method which shouldn't care about Publish status!
            var sql = "SELECT e.*, c.CommentCount, a.* " +
                      "FROM Entries AS e " +
                      "LEFT OUTER JOIN Authors AS a ON a.AuthorID = e.Author_AuthorID " +
                      "LEFT OUTER JOIN (SELECT Entry_EntryID, COUNT(*) as CommentCount " +
                      "FROM Comments GROUP BY Entry_EntryID) AS c " +
                      "ON e.EntryID = c.Entry_EntryID " +
                      "WHERE e.EntryID = @EntryID";

            var tagSql = "SELECT t.TagID, t.TagName " +
                         "FROM Tags AS t " +
                         "INNER JOIN EntryTags AS et ON et.Tag_TagID = t.TagID " +
                         "WHERE et.Entry_EntryID = @EntryID";

            var commentSql = "SELECT c.* " +
                             "FROM Comments AS c " +
                             "WHERE c.Entry_EntryID = @EntryID AND c.Approved = 1";

            var relatedSql = "SELECT e.EntryID, e.Title, e.Url, e.Body, e.Published " +
                             "FROM Entries AS e " +
                             "INNER JOIN EntryEntries AS ee ON ee.Entry_EntryID1 = e.EntryID " +
                             "WHERE ee.Entry_EntryID = @EntryID AND e.Publish = 1";

            Entry entry;

            using (var conn = base.GetOpenConnection())
            {
                using (_profiler.Step("Get entry by id (" + id + ")"))
                {
                    // Get the entry details
                    entry = conn.Query<Entry, Author, Entry>(sql, (e, a) => {
                        e.Author = a;
                        return e;
                    }, new {
                        EntryID = id
                    }, splitOn: "AuthorID").FirstOrDefault();
                }

                // Perform queries for the tags, comments and related entries
                using (_profiler.Step("Get tags for entry")) { entry.Tags = conn.Query<Tag>(tagSql, new { EntryID = entry.EntryID }).ToList(); }
                using (_profiler.Step("Get comments for entry")) { entry.Comments = conn.Query<Comment>(commentSql, new { EntryID = entry.EntryID }).ToList(); }
                using (_profiler.Step("Get related entries for entry"))
                {
                    entry.Related = conn.Query<Entry>(relatedSql, new { EntryID = entry.EntryID })
                                        .Select(x => {
                                            x.Thumbnail = GetThumbnail(x.Title, x.Body);
                                            return x;
                                        }).ToList();
                }
            }

            return entry;
        }

        public override void Add(Entry entry)
        {
            // There's only one author at the moment and this is unlikely to change,
            // so author assignment is hard-coded here for now
            var sql = "INSERT INTO Entries (Title, Body, Url, Published, Updated, Tweet, Publish, Author_AuthorID) " +
                      "VALUES (@Title, @Body, @Url, @Published, @Updated, @Tweet, @Publish, 1)";

            var newId = 0;

            entry.Published = DateTime.Now;
            entry.Updated = entry.Published;

            if (entry.Url == null && entry.Title != null)
                entry.Url = entry.Title.ConvertToSafeUrl();

            using (var conn = base.GetOpenConnection())
            {
                using (_profiler.Step("Add entry"))
                {
                    // We need to get the new ID with a separate query inside this transaction, because
                    // we want to support SQL CE, which doesn't support batch queries or SCOPE_IDENTITY()
                    using (var transaction = conn.BeginTransaction())
                    {
                        // Do the insert and retrieve the new ID
                        conn.Execute(sql, entry, transaction);
                        newId = (int)conn.Query<decimal>("SELECT @@IDENTITY", null, transaction).First();

                        transaction.Commit();
                    }
                }
            }

            entry.EntryID = newId;
        }

        public override void Update(Entry entry)
        {
            var sql = "UPDATE Entries " +
                      "SET Title = @Title, Body = @Body, Url = @Url, Updated = @Updated, Tweet = @Tweet, Publish = @Publish " +
                      "WHERE EntryID = @EntryID";

            entry.Updated = DateTime.Now;

            if (entry.Url == null && entry.Title != null)
                entry.Url = entry.Title.ConvertToSafeUrl();

            using (var conn = base.GetOpenConnection())
            {
                using (_profiler.Step("Update entry"))
                {
                    // Get the entry details
                    conn.Execute(sql, entry);
                }
            }
        }

        public void UpdateRelatedEntries(int entryId, int[] relatedIds)
        {
            using (var conn = base.GetOpenConnection())
            {
                using (_profiler.Step("Delete existing related entries"))
                {
                    // Get the entry details
                    conn.Execute("DELETE FROM EntryEntries WHERE Entry_EntryID = @EntryID", new { EntryID = entryId });
                }

                using (_profiler.Step("Update related entries"))
                {
                    var inserts = (from id in relatedIds
                                   select new { EntryID = entryId, RelatedID = id }).ToArray();

                    // Get the entry details
                    conn.Execute("INSERT INTO EntryEntries (Entry_EntryID, Entry_EntryID1) VALUES (@EntryID, @RelatedID)", inserts);
                }
            }
        }

        public void UpdateRelatedTags(int entryId, string[] tags)
        {
            using (var conn = base.GetOpenConnection())
            {
                using (_profiler.Step("Delete existing related tags"))
                {
                    // Get the entry details
                    conn.Execute("DELETE FROM EntryTags WHERE Entry_EntryID = @EntryID", new { EntryID = entryId });
                }

                using (_profiler.Step("Update entry"))
                {
                    var existingTags = conn.Query<Tag>("SELECT t.TagID, t.TagName from Tags as t WHERE t.TagName IN @Tags", new { Tags = tags });

                    var newTags = (from t in tags
                                   where !existingTags.Any(x => x.TagName == t)
                                   select new Tag { TagID = 0, TagName = t }).ToArray();

                    for (var i = 0; i < newTags.Length; i++ )
                    {
                        using (var transaction = conn.BeginTransaction())
                        {
                            // Do the insert and retrieve the new ID
                            conn.Execute("INSERT INTO Tags (TagName) VALUES (@TagName)", new { TagName = newTags[i].TagName }, transaction);
                            newTags[i].TagID = (int)conn.Query<decimal>("SELECT @@IDENTITY", null, transaction).First();

                            transaction.Commit();
                        }
                    }

                    var inserts = (from t in existingTags.Concat(newTags)
                                   select new { EntryID = entryId, TagID = t.TagID }).ToArray();

                    conn.Execute("INSERT INTO EntryTags (Entry_EntryID, Tag_TagID) VALUES (@EntryID, @TagID)", inserts);
                }
            }
        }

        public override void Remove(long id)
        {
            using (var conn = base.GetOpenConnection())
            {
                using (_profiler.Step("Delete entry"))
                {
                    conn.Execute("DELETE FROM EntryEntries WHERE Entry_EntryID = @EntryID", new { EntryID = id });
                    conn.Execute("DELETE FROM Entries WHERE EntryID = @EntryID", new { EntryID = id });
                }
            }
        }

        public IEnumerable<Entry> Like(string query)
        {
            var sql = "SELECT e.EntryID, e.Title, e.Published, e.Body, e.Url " +
                      "FROM Entries AS e " +
                      "WHERE e.Publish = 1 AND (e.Title LIKE @Query OR e.Body LIKE @Query)";

            IEnumerable<Entry> entries;

            using (var conn = base.GetOpenConnection())
            {
                using (_profiler.Step("Get entries matching '" + query + "'"))
                {
                    // Get the entries for this tag
                    entries = conn.Query<Entry>(sql, new { Query = "%" + query + "%" });
                }
            }

            return entries;
        }

        public void ClearErrorLogs(DateTime limit)
        {
            using (var conn = _elmahConnectionFactory.GetOpenConnection())
            {
                using (_profiler.Step("Delete error logs"))
                {
                    conn.Execute("DELETE FROM [ELMAH_Error] WHERE TimeUtc < @Limit", new { Limit = limit });
                }
            }
        }
    }
}
