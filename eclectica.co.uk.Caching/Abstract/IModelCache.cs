using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eclectica.co.uk.Caching.Abstract
{
    public interface IModelCache
    {
        List<KeyValuePair<string, object>> BaseCache { get; }
        object this[string key] { get; }
        void Add(string key, object value);
        T Get<T>(string key);
        void Clear();
    }
}
