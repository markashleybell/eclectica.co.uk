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
using eclectica.co.uk.Domain.Helpers;

namespace eclectica.co.uk.Service.Concrete
{
    public class EntryServices : IEntryServices
    {
        private IEntryRepository _entryRepository;
        private IAuthorRepository _authorRepository;
        private ICommentRepository _commentRepository;
        private IImageRepository _imageRepository;

        public EntryServices(IEntryRepository entryRepository, IAuthorRepository authorRepository, ICommentRepository commentRepository, IImageRepository imageRepository)
        {
            _entryRepository = entryRepository;
            _authorRepository = authorRepository;
            _commentRepository = commentRepository;
            _imageRepository = imageRepository;
        }

        public IEnumerable<EntryModel> All()
        {
            return Mapper.MapList<Entry, EntryModel>(_entryRepository.All().ToList());
        }

        public IEnumerable<EntryModel> Manage(bool? latest)
        {
            return Mapper.MapList<Entry, EntryModel>(_entryRepository.Manage(latest).ToList());
        }

        public EntryModel GetEntry(int id)
        {
            var entry = _entryRepository.Get(id);

            if (entry == null)
                return null;

            var model = Mapper.Map<Entry, EntryModel>(entry);
            model.Author = Mapper.Map<Author, AuthorModel>(entry.Author);
            model.Tags = Mapper.MapList<Tag, TagModel>(entry.Tags.ToList());
            model.Related = Mapper.MapList<Entry, EntryModel>(entry.Related.ToList());
            model.Comments = Mapper.MapList<Comment, CommentModel>(entry.Comments.ToList());

            return model;
        }

        public string[] GetEntryUrls()
        {
            return _entryRepository.GetEntryUrls();
        }

        public IEnumerable<string> GetUrlList()
        {
            return _entryRepository.UrlList();
        }

        public EntryModel GetEntryByUrl(string url)
        {
            var entry = _entryRepository.GetByUrl(url);

            if (entry == null)
                return null;

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

        public IEnumerable<ImageModel> GetImages()
        {
            return Mapper.MapList<Image, ImageModel>(_imageRepository.Last25().ToList());
        }

        public void AddImage(ImageModel model)
        {
            var image = new Image();

            Mapper.CopyProperties<ImageModel, Image>(model, image);

            _imageRepository.Add(image);

            model.ImageID = image.ImageID;
        }

        public IDictionary<DateTime, int> GetPostCountsPerMonth(int year)
        {
            var postCounts = _entryRepository.GetPostCounts(year);

            var months = new Dictionary<DateTime, int>();

            // Some months might not be represented in the repository results,
            // but we want them all present, even if there were no posts for a particular month
            for(var i = 1; i <= 12; i++)
            {
                var month = postCounts.Where(m => m.Key.Month == i).FirstOrDefault();

                if(month.Value != 0)
                    months.Add(month.Key, month.Value);
                else
                    months.Add(new DateTime(year, i, 1), 0);

            }

            return months;
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
            var entryDictionary = new Dictionary<string, List<EntryModel>>();

            var entryModels = from e in _entryRepository.GetByTag(tag)
                              orderby e.Published descending
                              select new EntryModel
                              {
                                  Url = e.Url,
                                  Published = e.Published,
                                  Title = e.Title,
                                  Body = ((string.IsNullOrEmpty(e.Title)) ? e.Body : "")
                              };

            foreach (var e in entryModels)
            {
                var date = e.Published.ToString("MMMM yyyy");

                if (!entryDictionary.ContainsKey(date))
                    entryDictionary.Add(date, new List<EntryModel>());

                entryDictionary[date].Add(e);
            }

            return entryDictionary;
        }

        public void AddEntry(EntryModel model, int[] relatedIds, string[] tags)
        {
            var entry = new Entry();

            Mapper.CopyProperties<EntryModel, Entry>(model, entry);

            _entryRepository.Add(entry);

            model.EntryID = entry.EntryID;

            if (relatedIds != null)
                _entryRepository.UpdateRelatedEntries(model.EntryID, relatedIds);

            if (tags != null)
                _entryRepository.UpdateRelatedTags(model.EntryID, tags);
        }

        public void UpdateEntry(EntryModel model, int[] relatedIds, string[] tags)
        {
            var entry = _entryRepository.Get(model.EntryID);

            Mapper.CopyProperties<EntryModel, Entry>(model, entry);

            if(relatedIds != null)
                _entryRepository.UpdateRelatedEntries(model.EntryID, relatedIds);

            if(tags != null)
                _entryRepository.UpdateRelatedTags(model.EntryID, tags);

            _entryRepository.Update(entry);
        }

        public void DeleteEntry(int id)
        {
            _entryRepository.Remove(id);
        }

        public IEnumerable<EntryModel> SimpleSearch(string query)
        {
            return Mapper.MapList<Entry, EntryModel>(_entryRepository.Like(query).ToList());
        }

        public void ClearErrorLogs(DateTime limit)
        {
            _entryRepository.ClearErrorLogs(limit);
        }

        public void CreateSearchIndex()
        {
            var indexPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Index");

            var writer = new DirectoryIndexWriter(new DirectoryInfo(indexPath), true);

            var entries = _entryRepository.All().Where(x => x.Publish).ToList();

            using (var indexService = new IndexService(writer))
            {
                indexService.IndexEntities(entries, e => {

                    var thumb = EntryHelpers.GetThumbnail(e.Title, e.Body, true);

                    var options = RegexOptions.Singleline | RegexOptions.IgnoreCase;

                    var summary = Regex.Replace(Regex.Matches(e.Body, "<p>(.*?)</p>", options)[0].Groups[1].Value, @"<(.|\n)*?>", string.Empty, options);

                    var document = new Document();
                    document.Add(new Field("url", e.Url, Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("published", e.Published.Ticks.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("title", (string.IsNullOrEmpty(e.Title) ? "" : e.Title), Field.Store.YES, Field.Index.ANALYZED));
                    document.Add(new Field("author", e.Author.Name, Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("body", e.Body.StripHtml(), Field.Store.YES, Field.Index.ANALYZED));
                    document.Add(new Field("thumbnail", thumb, Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("summary", summary, Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("commentcount", e.CommentCount.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    return document;

                });
            }
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
                    Query multiQuery = parser.Parse(keywords + "*");

                    this.AddQuery(multiQuery);
                }
                return this;
            }
        }
    }
}
