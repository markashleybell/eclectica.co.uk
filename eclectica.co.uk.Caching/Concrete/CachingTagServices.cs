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
    public class CachingTagServices : ITagServices
    {
        private ITagServices _tagServices;
        private IModelCache _cache;
        private ICacheConfigurationInfo _config;

        public CachingTagServices(ITagServices nonCachingTagServices, IModelCache cache, ICacheConfigurationInfo config)
        {
            _tagServices = nonCachingTagServices;
            _cache = cache;
            _config = config;
        }


        public IEnumerable<TagModel> All()
        {
            var key = "all-tags";
            var models = _cache.Get<IEnumerable<TagModel>>(key);

            if(models == null)
            {
                models = _tagServices.All();
                _cache.Add(key, models, _config.CacheIntervalMin);
            }

            return models;
        }

        public IEnumerable<TagModel> Search(string query)
        {
            return _tagServices.Search(query);
        }

        public Dictionary<string, List<TagModel>> GetSortedTags()
        {
            var key = "sorted-tags";
            var models = _cache.Get<Dictionary<string, List<TagModel>>>(key);

            if(models == null)
            {
                models = _tagServices.GetSortedTags();
                _cache.Add(key, models, _config.CacheIntervalMax);
            }

            return models;
        }
    }
}
