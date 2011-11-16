using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Caching.Abstract;
using System.Runtime.Caching;

namespace eclectica.co.uk.Caching.Concrete
{
    public class ModelCache : IModelCache
    {
        static MemoryCache _cache = MemoryCache.Default;

        public List<KeyValuePair<string, object>> BaseCache
        {
            get { return (from n in _cache.AsParallel() select n).ToList(); }
        }

        public object this[string key]
        {
            get { return _cache[key]; }
        }

        void IModelCache.Add(string key, object value)
        {
            _cache.Add(key, value, new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(60) });
        }

        T IModelCache.Get<T>(string key)
        {
            try
            {
                return (T)_cache[key];
            }
            catch
            {
                // TODO: Log cache miss here
                return default(T);
            }
        }

        void IModelCache.Clear()
        {
            List<KeyValuePair<String, Object>> cacheItems = (from n in _cache.AsParallel() select n).ToList();

            foreach (KeyValuePair<String, Object> a in cacheItems)
                _cache.Remove(a.Key);
        }
    }
}
