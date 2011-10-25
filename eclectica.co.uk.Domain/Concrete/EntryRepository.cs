using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Domain.Abstract;
using eclectica.co.uk.Domain.Entities;
using System.Linq.Expressions;
using System.Data;
using Dapper;

namespace eclectica.co.uk.Domain.Concrete
{
    public class EntryRepository : RepositoryBase<Entry>, IEntryRepository
    {
        public EntryRepository(IDbConnection connection) : base(connection) { }

        public Entry GetByUrl(string url)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Entry> All()
        {
            IEnumerable<Entry> entries;

            using (base.Connection)
            {
                base.Connection.Open();
                entries = base.Connection.Query<Entry>("select Title, Url, Published, Updated, Body from Entries");
            }

            return entries;
        }

        public IEnumerable<Entry> Page(int start, int count)
        {
            IEnumerable<Entry> entries;

            var query = "select Title, Url, Published, Updated, Body from Entries order by Published desc offset @Offset rows fetch next @Count rows only;";

            using (base.Connection)
            {
                base.Connection.Open();
                entries = base.Connection.Query<Entry>(query, new { Offset = start, Count = count });
            }

            return entries;
        }

        public override Entry Get(long id)
        {
            throw new NotImplementedException();
        }

        public override void Add(Entry entity)
        {
            throw new NotImplementedException();
        }

        public override void Remove(long id)
        {
            throw new NotImplementedException();
        }
    }
}
