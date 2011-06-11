using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Domain.Concrete;
using System.Data.Entity;
using System.Linq.Expressions;

namespace eclectica.co.uk.Domain.Abstract
{
    public abstract class RepositoryBase<T> where T : class
    {
        private readonly IDbSet<T> _dbset;
        private readonly IUnitOfWork _unitOfWork;

        protected RepositoryBase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dbset = _unitOfWork.Database.Set<T>();
        }

        public virtual IEnumerable<T> All()
        {
            return _dbset;
        }

        public virtual IEnumerable<T> Query(Expression<Func<T, bool>> filter)
        {
            return _dbset.Where(filter);
        }

        public virtual T Get(long id)
        {
            return _dbset.Find(id);
        }

        public virtual void Add(T entity)
        {
            _dbset.Add(entity);
        }

        public virtual void Remove(long id)
        {
            _dbset.Remove(_dbset.Find(id));
        }

    }
}
