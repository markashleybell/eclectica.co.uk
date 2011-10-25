﻿using System;
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

        public TagServices(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public IEnumerable<TagModel> All()
        {
            return Mapper.MapList<Tag, TagModel>(_tagRepository.All().ToList());
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

        public Dictionary<string, List<TagModel>> GetSortedTags()
        {
            var tagDictionary = new Dictionary<string, List<TagModel>>();

            var tagModels = from t in _tagRepository.All()
                            where t.Entries.Count > 0
                            orderby t.TagName
                            select new TagModel {
                                TagName = t.TagName,
                                UseCount = t.Entries.Count,
                                Class = GetTagClass(t.Entries.Count)
                            };

            foreach (var t in tagModels)
            {
                var first = t.TagName[0].ToString().ToUpper();

                if (!tagDictionary.ContainsKey(first))
                    tagDictionary.Add(first, new List<TagModel>());

                tagDictionary[first].Add(t);
            }

            return tagDictionary;
        }

        public Dictionary<string, List<EntryModel>> GetEntriesForTag(string tagName)
        {
            //var entryDictionary = new Dictionary<string, List<EntryModel>>();

            //var tag = _tagRepository.Query(x => x.TagName == tagName).FirstOrDefault();

            //if(tag != null)
            //{
            //    foreach (var e in tag.Entries.OrderByDescending(x => x.Published))
            //    {
            //        var date = e.Published.ToString("MMMM yyyy");

            //        if (!entryDictionary.ContainsKey(date))
            //            entryDictionary.Add(date, new List<EntryModel>());

            //        entryDictionary[date].Add(new EntryModel { 

            //            Title = e.Title,
            //            Published = e.Published,
            //            Url = e.Url,
            //            Body = (e.Title == "") ? e.Body : ""

            //        });
            //    }
            //}

            //return entryDictionary;

            throw new NotImplementedException();
        }
    }
}
