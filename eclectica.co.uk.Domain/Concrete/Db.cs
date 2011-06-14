using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using eclectica.co.uk.Domain.Entities;
using System.Data.Common;

namespace eclectica.co.uk.Domain.Concrete
{
    public class Db : DbContext
    {
        private IDbSet<Entry> _entries;
        private IDbSet<Comment> _comments;
        private IDbSet<Tag> _tags;
        private IDbSet<Image> _images;
        private IDbSet<Author> _authors;
        private IDbSet<Link> _links;

        public Db() : base()
        {
        }

        public Db(DbConnection connection) : base(connection, true)
        {
        }

        public IDbSet<Entry> Entries
        {
            get { return _entries ?? (_entries = DbSet<Entry>()); }
        }

        public IDbSet<Comment> Comments
        {
            get { return _comments ?? (_comments = DbSet<Comment>()); }
        }

        public IDbSet<Tag> Tags
        {
            get { return _tags ?? (_tags = DbSet<Tag>()); }
        }

        public IDbSet<Image> Images
        {
            get { return _images ?? (_images = DbSet<Image>()); }
        }

        public IDbSet<Author> Authors
        {
            get { return _authors ?? (_authors = DbSet<Author>()); }
        }

        public IDbSet<Link> Links
        {
            get { return _links ?? (_links = DbSet<Link>()); }
        }

        public virtual IDbSet<T> DbSet<T>() where T : class
        {
            return Set<T>();
        }

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entry>().HasMany(m => m.Related).WithMany();
            modelBuilder.Entity<Entry>().HasMany(m => m.Tags).WithMany();
        }
    }
}
