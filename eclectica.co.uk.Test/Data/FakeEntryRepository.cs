using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Domain.Abstract;
using eclectica.co.uk.Domain.Entities;

namespace eclectica.co.uk.Test.Data
{
    public class FakeEntryRepository : IEntryRepository
    {
        public List<Entry> _entries = new List<Entry> {
            new Entry {
                EntryID = 1,
                Author = new Author {
                    AuthorID = 1,
                    Email = "john.smith@test.com",
                    Name = "John Smith"
                },
                Title = "Test Entry 1",
                Url = "test-entry-1",
                Body = "<p>This is a test entry.</p>",
                Published = new DateTime(2011, 11, 10, 12, 30, 20),
                Updated = new DateTime(2011, 11, 12, 09, 15, 55),
                Publish = true,
                CommentCount = 2,
                Comments = new List<Comment> {
                    new Comment {
                        CommentID = 1,
                        EntryID = 1,
                        Name = "Jane Doe",
                        Email = "jane.doe@test.com",
                        Date = new DateTime(2011, 11, 11, 21, 45, 19),
                        Url = null,
                        Approved = true,
                        Body = "I like this entry.",
                        RawBody = "I like this entry."
                    },
                    new Comment {
                        CommentID = 2,
                        EntryID = 1,
                        Name = "Joe Bloggs",
                        Email = "joe.bloggs@test.com",
                        Date = new DateTime(2011, 11, 12, 16, 20, 33),
                        Url = "www.test.com",
                        Approved = true,
                        Body = "I like this entry too.",
                        RawBody = "I like this entry too."
                    }
                },
                Tags = new List<Tag> {
                    new Tag { TagID = 1, TagName = "test-items" },
                    new Tag { TagID = 2, TagName = "fakes" }
                },
                Related = new List<Entry> { }
            }
        };

        public Entry GetByUrl(string url)
        {
            return _entries.Where(x => x.Url == url).First();
        }

        public IEnumerable<string> UrlList()
        {
            throw new NotImplementedException();
        }

        public string GetRandomEntryUrl()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Entry> Page(int start, int count)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Entry> Month(int month, int year)
        {
            throw new NotImplementedException();
        }

        public IDictionary<DateTime, int> GetPostCounts(int year)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Entry> GetByTag(string tag)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Entry> GetLatest(int count)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Entry> Like(string query)
        {
            throw new NotImplementedException();
        }

        public void UpdateRelatedEntries(int entryId, int[] relatedIds)
        {
            throw new NotImplementedException();
        }

        public void UpdateRelatedTags(int entryId, string[] tags)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Entry> All()
        {
            return _entries;
        }

        public Entry Get(long id)
        {
            return _entries.Where(x => x.EntryID == id).First();
        }

        public void Add(Entry entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Entry entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(long id)
        {
            throw new NotImplementedException();
        }

        public void ClearErrorLogs(DateTime limit)
        {
            throw new NotImplementedException();
        }
    }
}
