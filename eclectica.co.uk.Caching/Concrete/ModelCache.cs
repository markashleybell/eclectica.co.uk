﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Caching.Abstract;
using System.Runtime.Caching;
using eclectica.co.uk.Caching.Entities;

namespace eclectica.co.uk.Caching.Concrete
{
    public class ModelCache : IModelCache
    {
        static MemoryCache _cache = MemoryCache.Default;
        static Dictionary<string, CacheItemInfo> _cacheInfo = new Dictionary<string,CacheItemInfo>();

        public List<CacheItemInfo> BaseCache
        {
            get 
            {
                return (from item in _cache
                        select _cacheInfo[item.Key]).ToList();
            }
        }

        public object this[string key]
        {
            get { return _cache[key]; }
        }

        void IModelCache.Add(string key, object value, int expirationSeconds)
        {
            _cache.Add(key, 
                value, 
            new CacheItemPolicy {
                AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(expirationSeconds) 
            });

            if(!_cacheInfo.ContainsKey(key))
            {
                _cacheInfo[key] = new CacheItemInfo {
                    Key = key,
                    Type = value.GetType().ToString(),
                    Hits = 0,
                    Misses = 0
                };
            }

            _cacheInfo[key].Misses++;
        }

        T IModelCache.Get<T>(string key)
        {
            try
            {
                _cacheInfo[key].Hits++;
                return (T)_cache[key];
            }
            catch
            {
                return default(T);
            }
        }

        void IModelCache.Remove(string key)
        {
            _cache.Remove(key);
            _cacheInfo.Remove(key);
        }

        void IModelCache.Clear()
        {
            List<KeyValuePair<String, Object>> cacheItems = (from n in _cache.AsParallel() select n).ToList();

            foreach (KeyValuePair<String, Object> a in cacheItems)
                _cache.Remove(a.Key);

            _cacheInfo.Clear();
        }
    }
}
