using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Service.Entities;

namespace eclectica.co.uk.Service.Abstract
{
    public interface IEntryServices
    {
        IEnumerable<EntryModel> All();
        EntryModel GetEntry(int id);
        EntryModel GetEntryByUrl(string folder);
        string GetRandomEntryUrl();

        IEnumerable<EntryModel> Page(int start, int count);
        IEnumerable<EntryModel> GetRecentEntries(int count);
        IEnumerable<EntryModel> GetArchivedEntries(int year, int month);

        IEnumerable<string> GetUrlList();

        IEnumerable<ImageModel> GetImages();
        void AddImage(ImageModel image);

        IDictionary<DateTime, int> GetPostCountsPerMonth(int year);

        IDictionary<string, List<EntryModel>> GetEntriesForTag(string tag);

        void ClearErrorLogs(DateTime limit);
        void CreateSearchIndex();
        IEnumerable<EntryModel> SearchEntries(string query);
        IEnumerable<EntryModel> SimpleSearch(string query);

        void AddEntry(EntryModel entry, int[] relatedIds, string[] tags);
        void UpdateEntry(EntryModel entry, int[] relatedIds, string[] tags);
        void DeleteEntry(int id);
    }
}
