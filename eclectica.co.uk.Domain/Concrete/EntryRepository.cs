﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Domain.Abstract;
using eclectica.co.uk.Domain.Entities;
using System.Linq.Expressions;
using System.Data;
using Dapper;
using MvcMiniProfiler;
using System.Text.RegularExpressions;
using System.Collections;

namespace eclectica.co.uk.Domain.Concrete
{
    public class EntryRepository : RepositoryBase<Entry>, IEntryRepository
    {
        private MiniProfiler _profiler = MiniProfiler.Current;

        public EntryRepository(IConnectionFactory connectionFactory) : base(connectionFactory) { }

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

            var relatedSql = "SELECT e.EntryID, e.Title, e.Url, e.Body " +
                             "FROM Entries AS e " +
                             "INNER JOIN EntryEntries AS ee ON ee.Entry_EntryID1 = e.EntryID " +
                             "WHERE ee.Entry_EntryID = @EntryID AND e.Publish = 1";

            Entry entry;

            using(var conn = base.GetOpenConnection())
            {
                using(_profiler.Step("Get entry by url"))
                {
                    // Get the entry and perform sub-queries for the tags, comments and related entries
                    entry = conn.Query<Entry, Author, Entry>(sql, (e, a) => {
                        e.Author = a;
                        using (_profiler.Step("Get tags for entry")) { e.Tags = conn.Query<Tag>(tagSql, new { EntryID = e.EntryID }).ToList(); }
                        using (_profiler.Step("Get comments for entry")) { e.Comments = conn.Query<Comment>(commentSql, new { EntryID = e.EntryID }).ToList(); }
                        using (_profiler.Step("Get related entries for entry"))
                        {
                            e.Related = conn.Query<Entry>(relatedSql, new { EntryID = e.EntryID })
                                            .Select(x => {
                                                x.Title = GetCaption(x.Title, x.Body);
                                                x.Thumbnail = GetThumbnail(x.Title, x.Body);
                                                return x;
                                            }).ToList();
                        }
                        return e;
                    }, new {
                        Url = url,
                    }, splitOn: "AuthorID").FirstOrDefault();
                }
            }

            return entry;
        }

        private string GetCaption(string title, string body)
        {
            var options = RegexOptions.Singleline | RegexOptions.IgnoreCase;
            return ((title == "") ? Regex.Replace(Regex.Matches(body, "<p>(.*?)</p>", options)[0].Groups[1].Value, @"<(.|\n)+?>", @"") : title);
        }

        private string GetThumbnail(string title, string body)
        {
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
            IEnumerable<Entry> entries;

            using (var conn = base.GetOpenConnection())
            {
                entries = conn.Query<Entry>("select Title, Url, Published, Updated, Body from Entries");
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

            using(var conn = base.GetOpenConnection())
            {
                using(_profiler.Step("Get entries for page"))
                {
                    // Get the entries for this page
                    entries = conn.Query<Entry, Author, Entry>(sql, (e, a) => {
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
                      "WHERE DATEPART(month, e.Published) = @Month AND DATEPART(year, e.Published) = @Year " + 
                      "ORDER BY e.Published";

            IEnumerable<Entry> entries;

            using (var conn = base.GetOpenConnection())
            {
                using (_profiler.Step("Get entries for month"))
                {
                    // Get the entries for this page
                    entries = conn.Query<Entry>(sql, new { Month = month, Year = year })
                                  .Select(x => {
                                      x.Title = GetCaption(x.Title, x.Body);
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
            return new List<Entry>();
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
