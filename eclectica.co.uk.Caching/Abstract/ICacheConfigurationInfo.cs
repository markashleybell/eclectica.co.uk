using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eclectica.co.uk.Caching.Abstract
{
    public interface ICacheConfigurationInfo
    {
        int CacheIntervalMin { get; }
        int CacheIntervalShort { get; }
        int CacheIntervalMedium { get; }
        int CacheIntervalLong { get; }
        int CacheIntervalMax { get; }
    }
}
