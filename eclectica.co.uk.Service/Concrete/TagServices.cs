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
    public class TagServices : ITagServices
    {
        private ITagRepository _tagRepository;
        private IUnitOfWork _unitOfWork;

        public TagServices(ITagRepository tagRepository, IUnitOfWork unitOfWork)
        {
            _tagRepository = tagRepository;
            _unitOfWork = unitOfWork;
        }

        private string GetTagClass(int usecount)
        {
            if (usecount < 3)
                return "xxs";
            else if (usecount < 5)
                return "xs";
            else if (usecount < 10)
                return "s";
            else if (usecount < 25)
                return "m";
            else if (usecount < 50)
                return "l";
            else if (usecount < 100)
                return "xl";
            else
                return "xxl";
        }

        public IEnumerable<TagModel> All()
        {
            var tmp = _tagRepository.All();

            var tagModels = from t in _tagRepository.All()
                            where t.Entries.Count > 0
                            orderby t.TagName
                            select new TagModel {
                                TagName = t.TagName,
                                UseCount = t.Entries.Count,
                                Class = GetTagClass(t.Entries.Count)
                            };

            return tagModels;
        }
    }
}
