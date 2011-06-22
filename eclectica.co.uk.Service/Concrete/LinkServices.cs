using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Service.Abstract;
using eclectica.co.uk.Domain.Abstract;
using eclectica.co.uk.Domain.Entities;
using mab.lib.SimpleMapper;
using eclectica.co.uk.Service.Entities;
using System.Data.Objects;

namespace eclectica.co.uk.Service.Concrete
{
    public class LinkServices : ILinkServices
    {
        private ILinkRepository _linkRepository;
        private IUnitOfWork _unitOfWork;

        public LinkServices(ILinkRepository linkRepository, IUnitOfWork unitOfWork)
        {
            _linkRepository = linkRepository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<LinkModel> All()
        {
            return Mapper.MapList<Link, LinkModel>(_linkRepository.All().ToList());
        }

        public IEnumerable<LinkModel> GetSortedLinks()
        {
            var linkDictionary = new Dictionary<string, List<LinkModel>>();

            var linkModels = from l in _linkRepository.All()
                             select new LinkModel {
                                 Title = l.Title,
                                 Url = l.Url,
                                 Category = l.Category
                             };

            return linkModels.ToList();
        }
    }
}
