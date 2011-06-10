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
        IEnumerable<T> Query(Expression<Func<T, bool>> filter);
        T Get(int id);
        void Add(T entity);
        void Remove(int id);
    }
}
