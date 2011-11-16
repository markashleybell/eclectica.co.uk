using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Service.Abstract;
using eclectica.co.uk.Service.Entities;
using eclectica.co.uk.Service.Concrete;
using eclectica.co.uk.Caching.Abstract;

namespace eclectica.co.uk.Caching.Concrete
{
    public class CachingEntryServices : IEntryServices
    {
        private IEntryServices _entryServices;
        private IModelCache _cache;

        public CachingEntryServices(IEntryServices nonCachingEntryServices, IModelCache cache)
        {
            _entryServices = nonCachingEntryServices;
            _cache = cache;
        }

        public IModelCache Cache
        {
            get { return _cache; }
        }

        public IEnumerable<EntryModel> All()
        {
            var key = "all-entries";
            var models = _cache.Get<IEnumerable<EntryModel>>(key);

            if(models == null)
            {
                models = _entryServices.All();
                _cache.Add(key, models);
            }

            return models;
        }

        public EntryModel GetEntry(int id)
        {
            throw new NotImplementedException();
        }

        public EntryModel GetEntryByUrl(string folder)
        {
            throw new NotImplementedException();
        }

        public string GetRandomEntryUrl()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EntryModel> Page(int start, int count)
        {
            var key = "page-" + start + "-" + count;
            var page = _cache.Get<IEnumerable<EntryModel>>(key);

            if (page == null)
            {
                page = _entryServices.Page(start, count);
                _cache.Add(key, page);
            }

            return page;
        }

        public IEnumerable<EntryModel> GetRecentEntries(int count)
        {
            var key = "recent-entries-" + count;
            var models = _cache.Get<IEnumerable<EntryModel>>(key);

            if(models == null)
            {
                models = _entryServices.GetRecentEntries(count);
                _cache.Add(key, models);
            }

            return models;
        }

        public IEnumerable<EntryModel> GetArchivedEntries(int year, int month)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetUrlList()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ImageModel> GetImages()
        {
            throw new NotImplementedException();
        }

        public void AddImage(ImageModel image)
        {
            throw new NotImplementedException();
        }

        public IDictionary<DateTime, int> GetPostCountsPerMonth(int year)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, List<EntryModel>> GetEntriesForTag(string tag)
        {
            throw new NotImplementedException();
        }

        public void ClearErrorLogs(DateTime limit)
        {
            throw new NotImplementedException();
        }

        public void CreateSearchIndex()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EntryModel> SearchEntries(string query)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EntryModel> SimpleSearch(string query)
        {
            throw new NotImplementedException();
        }

        public void AddEntry(EntryModel entry, int[] relatedIds, string[] tags)
        {
            throw new NotImplementedException();
        }

        public void UpdateEntry(EntryModel entry, int[] relatedIds, string[] tags)
        {
            throw new NotImplementedException();
        }

        public void DeleteEntry(int id)
        {
            throw new NotImplementedException();
        }
    }
}
