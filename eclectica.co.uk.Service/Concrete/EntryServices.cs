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
        private ICommentRepository _commentRepository;
        private IUnitOfWork _unitOfWork;

        public EntryServices(IEntryRepository entryRepository, ICommentRepository commentRepository, IUnitOfWork unitOfWork)
        {
            _entryRepository = entryRepository;
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

        public IEnumerable<EntryModel> Last(int count)
        {
            var entries = _entryRepository.All().OrderByDescending(x => x.Published).Take(count).ToList();

            var date = DateTime.Now.AddDays(-20);

            //var entries = _entryRepository.Query(x => x.Published > date).ToList();
            
            var entryModels = new List<EntryModel>();

            foreach(var entry in entries)
            {
                var entryModel = Mapper.Map<Entry, EntryModel>(entry);
                entryModel.Tags = new List<TagModel>(); // Mapper.MapList<Tag, TagModel>(entry.Tags.ToList());
                entryModel.Author = new AuthorModel { Name = "TEST" }; //  Mapper.Map<Author, AuthorModel>(entry.Author);
                entryModels.Add(entryModel);
            }

            return entryModels;
        }
    }
}
