﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Domain.Entities;

namespace eclectica.co.uk.Domain.Abstract
{
    public interface IEntryRepository : IRepository<Entry>
    {
        Entry GetByUrl(string url);
        string[] GetEntryUrls();
        IEnumerable<Entry> Page(int start, int count);
        IEnumerable<Entry> Month(int month, int year);
        IDictionary<DateTime, int> GetPostCounts(int year);
        IEnumerable<Entry> GetByTag(string tag);
        IEnumerable<Entry> GetLatest(int count);
        IEnumerable<Entry> Like(string query);
        IEnumerable<string> UrlList();
        IEnumerable<Entry> Manage(bool? latest);

        void UpdateRelatedEntries(int entryId, int[] relatedIds);
        void UpdateRelatedTags(int entryId, string[] tags);

        void ClearErrorLogs(DateTime limit);
    }
}
