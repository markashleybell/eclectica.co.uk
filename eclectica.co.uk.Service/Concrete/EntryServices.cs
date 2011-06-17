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

namespace eclectica.co.uk.Service.Concrete
{
    public class EntryServices : IEntryServices
    {
        private IEntryRepository _entryRepository;
        private IAuthorRepository _authorRepository;
        private ICommentRepository _commentRepository;
        private IUnitOfWork _unitOfWork;

        public EntryServices(IEntryRepository entryRepository, IAuthorRepository authorRepository, ICommentRepository commentRepository, IUnitOfWork unitOfWork)
        {
            _entryRepository = entryRepository;
            _authorRepository = authorRepository;
            _commentRepository = commentRepository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<EntryModel> All()
        {
            return Mapper.MapList<Entry, EntryModel>(_entryRepository.All().ToList());
        }

        public EntryModel GetEntry(int id)
        {
            return Mapper.Map<Entry, EntryModel>(_entryRepository.Get(id));
        }

        public EntryModel GetEntryByUrl(string url)
        {
            return Mapper.Map<Entry, EntryModel>(_entryRepository.GetByUrl(url));
        }

        public IEnumerable<EntryModel> Page(int start, int count)
        {
            var entryModels = from e in _entryRepository.All()
                              orderby e.Published descending
                              select new EntryModel
                              {
                                  EntryID = e.EntryID,
                                  Published = e.Published,
                                  Title = e.Title,
                                  Body = e.Body,
                                  Url = e.Url,
                                  Author = new AuthorModel
                                  {
                                      Name = e.Author.Name
                                  },
                                  Tags = Mapper.MapList<Tag, TagModel>(e.Tags.ToList()),
                                  CommentCount = (from c in _commentRepository.All() where c.Entry.EntryID == e.EntryID select c).Count()
                              };

            return entryModels.Skip(start).Take(count);
        }

        public IEnumerable<EntryModel> GetRecentEntries(int count)
        {
            var entryModels = from e in _entryRepository.All()
                              orderby e.Published descending
                              select new EntryModel
                              {
                                  Url = e.Url,
                                  Published = e.Published,
                                  Title = e.Title
                              };

            return entryModels.Take(count);
        }

        public Dictionary<string, List<EntryModel>> GetEntriesForTag(string tag)
        {
            var entryDictionary = new Dictionary<string, List<EntryModel>>();

            var entryModels = from e in _entryRepository.All()
                              where e.Tags.Any(t => t.TagName == tag)
                              orderby e.Published descending
                              select new EntryModel
                              {
                                  Url = e.Url,
                                  Published = e.Published,
                                  Title = e.Title,
                                  Body = (e.Title == "") ? e.Body : ""
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

        public void CreateSearchIndex()
        {
            var indexPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Index");

            var writer = new DirectoryIndexWriter(new DirectoryInfo(indexPath), true);

            using (var indexService = new IndexService(writer))
            {
                indexService.IndexEntities(_entryRepository.All().ToList(), e => {
                    var document = new Document();
                    document.Add(new Field("url", e.Url, Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("published", e.Published.Ticks.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("title", e.Title, Field.Store.YES, Field.Index.ANALYZED));
                    document.Add(new Field("body", e.Body.StripHtml(), Field.Store.YES, Field.Index.ANALYZED));
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

            IList<EntryModel> results = searchService.SearchIndex(q.Query, new EntryResultDefinition()).Results.ToList();

            return results;
        }

        public class EntryResultDefinition : IResultDefinition<EntryModel>
        {
            public EntryModel Convert(Document document)
            {
                return new EntryModel
                {
                    Title = document.GetValue("title"),
                    Url = document.GetValue("url"),
                    Published = new DateTime(document.GetValue<long>("published"))
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
