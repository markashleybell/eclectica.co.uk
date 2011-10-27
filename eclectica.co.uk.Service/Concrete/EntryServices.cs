using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Service.Abstract;
using eclectica.co.uk.Domain.Abstract;
using eclectica.co.uk.Domain.Entities;
using mab.lib.SimpleMapper;
using eclectica.co.uk.Service.Entities;
using Lucene.Net.Documents;
using SimpleLucene;
using Lucene.Net.Index;
using System.IO;
using SimpleLucene.Impl;
using Lucene.Net.Search;
using eclectica.co.uk.Service.Extensions;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.QueryParsers;
using System.Text.RegularExpressions;

namespace eclectica.co.uk.Service.Concrete
{
    public class EntryServices : IEntryServices
    {
        private IEntryRepository _entryRepository;
        private IAuthorRepository _authorRepository;
        private ICommentRepository _commentRepository;

        public EntryServices(IEntryRepository entryRepository, IAuthorRepository authorRepository, ICommentRepository commentRepository)
        {
            _entryRepository = entryRepository;
            _authorRepository = authorRepository;
            _commentRepository = commentRepository;
        }

        public IEnumerable<EntryModel> All()
        {
            return Mapper.MapList<Entry, EntryModel>(_entryRepository.All().ToList());
        }

        public EntryModel GetEntry(int id)
        {
            return Mapper.Map<Entry, EntryModel>(_entryRepository.Get(id));
        }

        public string GetRandomEntryUrl()
        {
            //var entries = (from e in _entryRepository.Query(x => x.Publish == true)
            //               select e.Url).ToList();

            //int random = new Random().Next(0, entries.Count);

            //return entries[random];

            throw new NotImplementedException();
        }

        public EntryModel GetEntryByUrl(string url)
        {
            var entry = _entryRepository.GetByUrl(url);

            var model = Mapper.Map<Entry, EntryModel>(entry);
            model.Author = Mapper.Map<Author, AuthorModel>(entry.Author);
            model.Tags = Mapper.MapList<Tag, TagModel>(entry.Tags.ToList());
            model.Related = Mapper.MapList<Entry, EntryModel>(entry.Related.ToList());
            model.Comments = Mapper.MapList<Comment, CommentModel>(entry.Comments.ToList());

            return model;
        }

        public IEnumerable<EntryModel> Page(int start, int count)
        {
            var entries = _entryRepository.Page(start, count);

            var models = new List<EntryModel>();

            foreach(var entry in entries)
            {
                var model = Mapper.Map<Entry, EntryModel>(entry);
                model.Author = Mapper.Map<Author, AuthorModel>(entry.Author);
                model.Tags = Mapper.MapList<Tag, TagModel>(entry.Tags.ToList());
                models.Add(model);
            }

            return models;
        }

        public IDictionary<DateTime, int> GetPostCountsPerMonth(int year)
        {
            return _entryRepository.GetPostCounts(year);
        }

        public IEnumerable<EntryModel> GetArchivedEntries(int year, int month)
        {
            var entries = _entryRepository.Month(month, year);

            return Mapper.MapList<Entry, EntryModel>(entries.ToList());
        }

        public IEnumerable<EntryModel> GetRecentEntries(int count)
        {
            return Mapper.MapList<Entry, EntryModel>(_entryRepository.GetLatest(10).ToList());
        }

        public IDictionary<string, List<EntryModel>> GetEntriesForTag(string tag)
        {
            //var entryDictionary = new Dictionary<string, List<EntryModel>>();

            //var entryModels = from e in _entryRepository.Query(x => x.Publish == true)
            //                  where e.Tags.Any(t => t.TagName == tag)
            //                  orderby e.Published descending
            //                  select new EntryModel
            //                  {
            //                      Url = e.Url,
            //                      Published = e.Published,
            //                      Title = e.Title,
            //                      Body = (e.Title == "") ? e.Body : ""
            //                  };

            //foreach (var e in entryModels)
            //{
            //    var date = e.Published.ToString("MMMM yyyy");

            //    if (!entryDictionary.ContainsKey(date))
            //        entryDictionary.Add(date, new List<EntryModel>());

            //    entryDictionary[date].Add(e);
            //}

            //return entryDictionary;

            throw new NotImplementedException();
        }

        public void UpdateEntry(EntryModel entry)
        {

        }

        public void CreateSearchIndex()
        {
            //var indexPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Index");

            //var writer = new DirectoryIndexWriter(new DirectoryInfo(indexPath), true);

            //using (var indexService = new IndexService(writer))
            //{
            //    indexService.IndexEntities(_entryRepository.Query(x => x.Publish == true).ToList(), e =>
            //    {

            //        string body = e.Body;
            //        RegexOptions options = RegexOptions.Singleline | RegexOptions.IgnoreCase;

            //        string summary = Regex.Replace(Regex.Matches(body, "<p>(.*?)</p>", options)[0].Groups[1].Value, @"<(.|\n)*?>", string.Empty, options);

            //        MatchCollection imgElements = Regex.Matches(body, "<img (?:.*?)?src=\"/content/img/lib/(.*?)/(.*?)\\.(jpg|gif)\" (?:.*?)?/>", options);

            //        string thumb = "";

            //        if (imgElements.Count > 0)
            //        {
            //            Match img = imgElements[0];

            //            // If it's a drop image it won't have a small image
            //            if (img.Groups[1].Value == "drop")
            //            {
            //                // Create one on the fly?
            //            }
            //            else
            //            {
            //                thumb = img.Groups[2].Value + "." + img.Groups[3].Value;
            //            }
            //        }

            //        var document = new Document();
            //        document.Add(new Field("url", e.Url, Field.Store.YES, Field.Index.NOT_ANALYZED));
            //        document.Add(new Field("published", e.Published.Ticks.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            //        document.Add(new Field("title", e.Title, Field.Store.YES, Field.Index.ANALYZED));
            //        document.Add(new Field("author", e.Author.Name, Field.Store.YES, Field.Index.NOT_ANALYZED));
            //        document.Add(new Field("body", e.Body.StripHtml(), Field.Store.YES, Field.Index.ANALYZED));
            //        document.Add(new Field("thumbnail", thumb, Field.Store.YES, Field.Index.NOT_ANALYZED));
            //        document.Add(new Field("summary", summary, Field.Store.YES, Field.Index.NOT_ANALYZED));
            //        document.Add(new Field("commentcount", e.Comments.Count.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            //        return document;
            //    });
            //}

            throw new NotImplementedException();
        }

        public IEnumerable<EntryModel> SearchEntries(string query)
        {
            var indexPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Index");

            var searcher = new DirectoryIndexSearcher(new DirectoryInfo(indexPath), true);

            var q = new EntryQuery().WithKeywords(query);

            var searchService = new SearchService(searcher);

            IList<EntryModel> results = searchService.SearchIndex(q.Query, new EntryResultDefinition(query)).Results.ToList();

            return results;
        }

        public class EntryResultDefinition : IResultDefinition<EntryModel>
        {
            private string _query;

            public EntryResultDefinition(string query)
            {
                _query = query;
            }

            public EntryModel Convert(Document document)
            {
                return new EntryModel
                {
                    Title = document.GetValue("title"),
                    Url = document.GetValue("url"),
                    Author = new AuthorModel { Name = document.GetValue("author") },
                    Published = new DateTime(document.GetValue<long>("published")),
                    Body = Regex.Replace(document.GetValue("summary"), "(" + _query + ")", "<b>$1</b>", RegexOptions.Singleline | RegexOptions.IgnoreCase),
                    Thumbnail = document.GetValue("thumbnail"),
                    CommentCount = document.GetValue<int>("commentcount")
                };
            }
        }

        public class EntryQuery : QueryBase
        {
            public EntryQuery(Query query) : base(query) { }

            public EntryQuery() { }

            public EntryQuery WithKeywords(string keywords)
            {
                if (!string.IsNullOrEmpty(keywords))
                {
                    string[] fields = { "title", "body" };
                    var parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_29,
                        fields, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29));
                    Query multiQuery = parser.Parse(keywords);

                    this.AddQuery(multiQuery);
                }
                return this;
            }
        }
    }
}
