using System;

namespace TaskManager.BLL.Infrastructure
{
    public static class SearchTermFormat
    {
        private static string SanitizeSearchTerm(string searchPhrase)
        {
            if (searchPhrase.Length > 200)
                searchPhrase = searchPhrase.Substring(0, 200);
            searchPhrase = searchPhrase.Replace(";", " ");
            searchPhrase = searchPhrase.Replace("'", " ");
            searchPhrase = searchPhrase.Replace("--", " ");
            searchPhrase = searchPhrase.Replace("/*", " ");
            searchPhrase = searchPhrase.Replace("*/", " ");
            searchPhrase = searchPhrase.Replace("xp_", " ");
            return searchPhrase;
        }
        public static string[] ToTermsArray(this string searchTerm)
        {
            var searchTerms = SanitizeSearchTerm(searchTerm.Trim()).Split(new [] {' '}, StringSplitOptions.RemoveEmptyEntries);
            
            return searchTerms;
        }
    }
}
