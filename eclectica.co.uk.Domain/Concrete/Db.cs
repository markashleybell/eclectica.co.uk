using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using eclectica.co.uk.Domain.Entities;

namespace eclectica.co.uk.Domain.Concrete
{
    public class Db : DbContext
    {
        private IDbSet<Entry> _entries;
        private IDbSet<Comment> _comments;

        public IDbSet<Entry> Entries
        {
            get { return _entries ?? (_entries = DbSet<Entry>()); }
        }

        public IDbSet<Comment> Comments
        {
            get { return _comments ?? (_comments = DbSet<Comment>()); }
        }

        public virtual IDbSet<T> DbSet<T>() where T : class
        {
            return Set<T>();
        }

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.Edm.Db.ColumnTypeCasingConvention>();

        //    modelBuilder.Entity<Article>()
        //                .Property(p => p.Body)
        //                .HasColumnType("varchar(max)");
        //}
    }
}
