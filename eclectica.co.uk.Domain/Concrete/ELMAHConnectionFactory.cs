using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MvcMiniProfiler.Data;
using System.Data.SqlServerCe;
using MvcMiniProfiler;
using System.Data.Common;
using eclectica.co.uk.Domain.Abstract;
using eclectica.co.uk.Domain.Entities;

namespace eclectica.co.uk.Domain.Concrete
{
    public class ELMAHConnectionFactory : IELMAHConnectionFactory
    {
        private string _connectionString;

        public ELMAHConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection GetOpenConnection()
        {
            var connection = new ProfiledDbConnection((DbConnection)new SqlCeConnection(_connectionString), MiniProfiler.Current);
            connection.Open();

            return connection;
        }
    }
}
