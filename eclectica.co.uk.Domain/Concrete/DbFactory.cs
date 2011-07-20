using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Domain.Extensions;
using eclectica.co.uk.Domain.Abstract;
using MvcMiniProfiler.Data;
using System.Data.SqlServerCe;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;

namespace eclectica.co.uk.Domain.Concrete
{
    public class DbFactory : Disposable, IDbFactory
    {
        private Db _database;

        public Db Get()
        {
            _database = new Db();

            return _database;
        }

        protected override void DisposeCore()
        {
            if (_database != null) _database.Dispose();
        }
    }
}
