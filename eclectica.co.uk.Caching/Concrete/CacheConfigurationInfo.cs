using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Caching.Abstract;

namespace eclectica.co.uk.Caching.Concrete
{
    public class CacheConfigurationInfo : ICacheConfigurationInfo
    {
        private int _cacheIntervalMin;
        private int _cacheIntervalShort;
        private int _cacheIntervalMedium;
        private int _cacheIntervalLong;
        private int _cacheIntervalMax;

        public int CacheIntervalMin { get { return _cacheIntervalMin; } }
        public int CacheIntervalShort { get { return _cacheIntervalShort; } }
        public int CacheIntervalMedium { get { return _cacheIntervalMedium; } }
        public int CacheIntervalLong { get { return _cacheIntervalLong; } }
        public int CacheIntervalMax { get { return _cacheIntervalMax; } }

        public CacheConfigurationInfo(int cacheIntervalMin, 
                                      int cacheIntervalShort, 
                                      int cacheIntervalMedium, 
                                      int cacheIntervalLong, 
                                      int cacheIntervalMax)
        {
            _cacheIntervalMin = cacheIntervalMin;
            _cacheIntervalShort = cacheIntervalShort;
            _cacheIntervalMedium = cacheIntervalMedium;
            _cacheIntervalLong = cacheIntervalLong;
            _cacheIntervalMax = cacheIntervalMax;
        }
    }
}
