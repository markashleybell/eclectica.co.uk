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
        private readonly IDbConnection _connection;

        protected RepositoryBase(IDbConnection connection)
        {
            _connection = new ProfiledDbConnection((DbConnection)connection, MiniProfiler.Current);
        }

        public IDbConnection Connection
        {
            get { return _connection; }
        }

        public abstract IEnumerable<T> All();
        public abstract T Get(long id);
        public abstract void Add(T entity);
        public abstract void Remove(long id);

    }
}
