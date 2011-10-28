using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using eclectica.co.uk.Domain.Entities;

namespace eclectica.co.uk.Domain.Abstract
{
    public interface IConnectionFactory
    {
        IDbConnection GetOpenConnection();
        DbServerType ServerType { get; }
    }
}
