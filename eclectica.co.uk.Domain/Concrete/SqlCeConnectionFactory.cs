﻿using System;
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
    public class SqlCeConnectionFactory : IConnectionFactory
    {
        private string _connectionString;
        private DbServerType _serverType;

        public SqlCeConnectionFactory(string connectionString, DbServerType serverType)
        {
            _connectionString = connectionString;
            _serverType = serverType;
        }

        public DbServerType ServerType { get { return _serverType; } }

        public IDbConnection GetOpenConnection()
        {
            var connection = new ProfiledDbConnection((DbConnection)new SqlCeConnection(_connectionString), MiniProfiler.Current);
            connection.Open();

            return connection;
        }
    }
}
