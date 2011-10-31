using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace eclectica.co.uk.Domain.Abstract
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> All();
        T Get(long id);
        void Add(T entity);
        void Update(T entity);
        void Remove(long id);
    }
}
