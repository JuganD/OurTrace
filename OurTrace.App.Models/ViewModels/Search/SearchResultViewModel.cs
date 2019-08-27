using System.Collections.Generic;

namespace OurTrace.App.Models.ViewModels.Search
{
    public class SearchResultViewModel
    {
        public string Content { get; set; }
        public string DescriptiveContent { get; set; }
        public ICollection<string> ContextVariables { get; set; }
    }
    public enum SearchResultType
    {
        User = 1,
        Group = 2,
        Post = 3,
        Comment = 4
    }
}
