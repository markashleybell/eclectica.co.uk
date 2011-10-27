using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Domain.Concrete;
using System.Linq.Expressions;
using System.Data;
using Dapper;
using eclectica.co.uk.Domain.Entities;
using MvcMiniProfiler;
using MvcMiniProfiler.Data;
using System.Data.Common;

namespace eclectica.co.uk.Domain.Abstract
{
    public abstract class RepositoryBase<T> where T : class
    {
        private readonly IConnectionFactory _connectionFactory;

        protected RepositoryBase(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IDbConnection GetOpenConnection()
        {
            return _connectionFactory.GetOpenConnection();
        }

        public abstract IEnumerable<T> All();
        public abstract T Get(long id);
        public abstract void Add(T entity);
        public abstract void Remove(long id);

    }
}
