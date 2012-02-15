using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using eclectica.co.uk.Web.Controllers;
using eclectica.co.uk.Web.Concrete;
using eclectica.co.uk.Service.Concrete;
using eclectica.co.uk.Domain.Concrete;
using eclectica.co.uk.Test.Data;

namespace eclectica.co.uk.Test
{
    [TestFixture]
    public class EntryControllerTests
    {
        private ConfigurationInfo _config = new ConfigurationInfo("", "", "", 5, "", "", "", "", "", "", "", "", "", "", "");

        [Test]
        public void Get_Entry_Details_By_Id()
        {
            var entryServices = new EntryServices(new FakeEntryRepository(), 
                                                  new FakeAuthorRepository(), 
                                                  new FakeCommentRepository(), 
                                                  new FakeImageRepository());

            var entry = entryServices.GetEntry(1);

            entry.EntryID.ShouldEqual(1);
            entry.Title.ShouldEqual("Test Entry 1");
            entry.Comments.ShouldEqual(null);
        }

        [Test]
        public void Get_Entry_Details_By_Url()
        {
            var entryServices = new EntryServices(new FakeEntryRepository(),
                                                  new FakeAuthorRepository(),
                                                  new FakeCommentRepository(),
                                                  new FakeImageRepository());

            var entry = entryServices.GetEntryByUrl("test-entry-1");

            entry.EntryID.ShouldEqual(1);
            entry.Title.ShouldEqual("Test Entry 1");
            entry.Comments[1].CommentID.ShouldEqual(2);
        }
    }
}
