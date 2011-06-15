using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Service.Abstract;
using eclectica.co.uk.Domain.Abstract;
using eclectica.co.uk.Domain.Entities;
using mab.lib.SimpleMapper;
using eclectica.co.uk.Service.Entities;

namespace eclectica.co.uk.Service.Concrete
{
    public class EntryServices : IEntryServices
    {
        private IEntryRepository _entryRepository;
        private IAuthorRepository _authorRepository;
        private ICommentRepository _commentRepository;
        private IUnitOfWork _unitOfWork;

        public EntryServices(IEntryRepository entryRepository, IAuthorRepository authorRepository, ICommentRepository commentRepository, IUnitOfWork unitOfWork)
        {
            _entryRepository = entryRepository;
            _authorRepository = authorRepository;
            _commentRepository = commentRepository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<EntryModel> All()
        {
            return Mapper.MapList<Entry, EntryModel>(_entryRepository.All().ToList());
        }

        public EntryModel GetEntry(int id)
        {
            return Mapper.Map<Entry, EntryModel>(_entryRepository.Get(id));
        }

        public EntryModel GetEntryByUrl(string url)
        {
            return Mapper.Map<Entry, EntryModel>(_entryRepository.GetByUrl(url));
        }

        public IEnumerable<EntryModel> Page(int start, int count)
        {
            var entryModels = from e in _entryRepository.All()
                              orderby e.Published descending
                              select new EntryModel
                              {
                                  EntryID = e.EntryID,
                                  Published = e.Published,
                                  Title = e.Title,
                                  Body = e.Body,
                                  Url = e.Url,
                                  Author = new AuthorModel
                                  {
                                      Name = e.Author.Name
                                  },
                                  Tags = Mapper.MapList<Tag, TagModel>(e.Tags.ToList()),
                                  CommentCount = (from c in _commentRepository.All() where c.Entry.EntryID == e.EntryID select c).Count()
                              };

            return entryModels.Skip(start).Take(count);
        }

        public IEnumerable<EntryModel> GetRecentEntries(int count)
        {
            var entryModels = from e in _entryRepository.All()
                              orderby e.Published descending
                              select new EntryModel
                              {
                                  Url = e.Url,
                                  Published = e.Published,
                                  Title = e.Title
                              };

            return entryModels.Take(count);
        }
    }
}
