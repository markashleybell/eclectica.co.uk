using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Domain.Extensions;
using eclectica.co.uk.Domain.Abstract;
using MvcMiniProfiler.Data;
using System.Data.SqlServerCe;

namespace eclectica.co.uk.Domain.Concrete
{
    public class DbFactory : Disposable, IDbFactory
    {
        private Db _database;

        public Db Get()
        {
            // This stuff will not work with EF 4.1 yet due to a bug in the mini profiler code
            // var sqlConnection = new SqlCeConnection(@"Data Source=E:\Inetpub\myapps\eclectica.co.uk\eclectica.co.uk.Web\App_Data\eclectica.sdf");
            // var profiledConnection = ProfiledDbConnection.Get(sqlConnection);

            // _database = new Db(profiledConnection);

            _database = new Db();

            return _database;
        }

        protected override void DisposeCore()
        {
            if (_database != null) _database.Dispose();
        }
    }
}
