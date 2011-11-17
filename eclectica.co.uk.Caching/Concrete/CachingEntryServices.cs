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
        private ICacheConfigurationInfo _config;

        public CachingEntryServices(IEntryServices nonCachingEntryServices, IModelCache cache, ICacheConfigurationInfo config)
        {
            _entryServices = nonCachingEntryServices;
            _cache = cache;
            _config = config;
        }

        public IEnumerable<EntryModel> All()
        {
            var key = "all-entries";
            var models = _cache.Get<IEnumerable<EntryModel>>(key);

            if(models == null)
            {
                models = _entryServices.All();
                _cache.Add(key, models, _config.CacheIntervalMin);
            }

            return models;
        }

        public EntryModel GetEntry(int id)
        {
            // Return this uncached, as it's what we use for the CMS and previewing
            return _entryServices.GetEntry(id);
        }

        public EntryModel GetEntryByUrl(string folder)
        {
            var key = "entry-" + folder;
            var entry = _cache.Get<EntryModel>(key);

            if(entry == null)
            {
                entry = _entryServices.GetEntryByUrl(folder);
                _cache.Add(key, entry, _config.CacheIntervalMedium);
            }

            return entry;
        }

        public string[] GetEntryUrls()
        {
            var key = "all-urls";
            var urls = _cache.Get<string[]>(key);

            if(urls == null)
            {
                urls = _entryServices.GetEntryUrls();
                _cache.Add(key, urls, _config.CacheIntervalMax);
            }

            return urls;
        }

        public IEnumerable<EntryModel> Page(int start, int count)
        {
            var key = "page-" + start + "-" + count;
            var page = _cache.Get<IEnumerable<EntryModel>>(key);

            if (page == null)
            {
                page = _entryServices.Page(start, count);
                _cache.Add(key, page, _config.CacheIntervalLong);
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
                _cache.Add(key, models, _config.CacheIntervalMax);
            }

            return models;
        }

        public IEnumerable<EntryModel> GetArchivedEntries(int year, int month)
        {
            var key = "archive-" + year + "-" + month;
            var models = _cache.Get<IEnumerable<EntryModel>>(key);

            if(models == null)
            {
                models = _entryServices.GetArchivedEntries(year, month);
                _cache.Add(key, models, _config.CacheIntervalLong);
            }

            return models;
        }

        public IEnumerable<string> GetUrlList()
        {
            // Don't bother caching this as it's only ever called by the sitemap and 
            // we want that to be fresh anyway
            return _entryServices.GetUrlList();
        }

        public IEnumerable<ImageModel> GetImages()
        {
            return _entryServices.GetImages();
        }

        public void AddImage(ImageModel image)
        {
            _entryServices.AddImage(image);
        }

        public IDictionary<DateTime, int> GetPostCountsPerMonth(int year)
        {
            var key = "postcounts-" + year;
            var models = _cache.Get<IDictionary<DateTime, int>>(key);

            if(models == null)
            {
                models = _entryServices.GetPostCountsPerMonth(year);
                _cache.Add(key, models, _config.CacheIntervalLong);
            }

            return models;
        }

        public IDictionary<string, List<EntryModel>> GetEntriesForTag(string tag)
        {
            var key = "tagentries-" + tag;
            var models = _cache.Get<IDictionary<string, List<EntryModel>>>(key);

            if(models == null)
            {
                models = _entryServices.GetEntriesForTag(tag);
                _cache.Add(key, models, _config.CacheIntervalLong);
            }

            return models;
        }

        public void ClearErrorLogs(DateTime limit)
        {
            _entryServices.ClearErrorLogs(limit);   
        }

        public void CreateSearchIndex()
        {
            _entryServices.CreateSearchIndex();
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
            _entryServices.AddEntry(entry, relatedIds, tags);
        }

        public void UpdateEntry(EntryModel entry, int[] relatedIds, string[] tags)
        {
            _entryServices.UpdateEntry(entry, relatedIds, tags);
        }

        public void DeleteEntry(int id)
        {
            _entryServices.DeleteEntry(id);
        }
    }
}
