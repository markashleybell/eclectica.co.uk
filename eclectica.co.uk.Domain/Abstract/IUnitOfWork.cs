using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Domain.Concrete;

namespace eclectica.co.uk.Domain.Abstract
{
    public interface IUnitOfWork
    {
        Db Database { get; }
        void Commit();
    }
}
