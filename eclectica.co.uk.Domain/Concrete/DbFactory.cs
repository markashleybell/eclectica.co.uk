using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Domain.Extensions;
using eclectica.co.uk.Domain.Abstract;
using MvcMiniProfiler.Data;
using System.Data.SqlClient;

namespace eclectica.co.uk.Domain.Concrete
{
    public class DbFactory : Disposable, IDbFactory
    {
        private Db _database;

        public Db Get()
        {
            _database = new Db();

            var sqlConnection = new SqlConnection(@"Data Source=C:\Web\eclectica.co.uk\eclectica.co.uk.Web\App_Data\ec-2011js7i3.sdf");
            var profiledConnection = ProfiledDbConnection.Get(sqlConnection);

            // ProfiledDbConnection.Get(_database.Connection);

            return _database;
        }

        protected override void DisposeCore()
        {
            if (_database != null) _database.Dispose();
        }
    }
}
