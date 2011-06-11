using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Infrastructure;
using eclectica.co.uk.Domain.Concrete;
using eclectica.co.uk.Domain.Entities;
using System.Data.Entity;

namespace eclectica.co.uk.Service.Configuration
{
    public static class Initialization
    {
        public static void InitializeDb()
        {
            Database.SetInitializer<Db>(new DbInitializer());
        }
    }

    public class DbInitializer : DropCreateDatabaseIfModelChanges<Db>
    {
        protected override void Seed(Db context)
        {
            var entry = new Entry
            {
                Title = "Test Article",
                Author = new Author { 
                    Name = "Mark Bell",
                    Email = "markb@eclectica.co.uk"
                },
                Body = "<p>This is the test article text.</p>",
                Url = "test-article",
                Published = DateTime.Now,
                Updated = DateTime.Now,
                Comments = new List<Comment> 
                {
                    new Comment 
                    {
                        Name = "Bob Jones",
                        Email = "bob@bob.com",
                        Url = "http://jim.com/",
                        Body = "This is a test",
                        RawBody = "<p>This is a test</p>",
                        Date = DateTime.Now
                    },
                    new Comment 
                    {
                        Name = "Jim Smith",
                        Email = "jim@jim.com",
                        Url = "",
                        Body = "Jim says hello!",
                        RawBody = "<p>Jim says hello!</p>",
                        Date = DateTime.Now
                    }
                }
            };

            var entry2 = new Entry
            {
                Title = "Test Article 2",
                Author = new Author { 
                    Name = "Mark Bell",
                    Email = "markb@eclectica.co.uk"
                },
                Body = "<p>This is the test article 2 text.</p>",
                Url = "test-article-2",
                Published = DateTime.Now,
                Updated = DateTime.Now,
                Comments = new List<Comment> 
                {
                    new Comment 
                    {
                        Name = "Jim Smith",
                        Email = "jim@jim.com",
                        Url = "",
                        Body = "Jim says hello!",
                        RawBody = "<p>Jim says hello!</p>",
                        Date = DateTime.Now
                    },
                    new Comment 
                    {
                        Name = "Bob Jones",
                        Email = "bob@bob.com",
                        Url = "http://jim.com/",
                        Body = "This is a test",
                        RawBody = "<p>This is a test</p>",
                        Date = DateTime.Now
                    }
                }
            };

            context.Entries.Add(entry);
            context.Entries.Add(entry2);

            context.Images.Add(new Image { Filename = "test.jpg" });
        }
    }
}