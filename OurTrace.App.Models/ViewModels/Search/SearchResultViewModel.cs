using System.Collections.Generic;

namespace OurTrace.App.Models.ViewModels.Search
{
    public class SearchResultViewModel
    {
        public SearchResultViewModel()
        {
            this.ContextVariables = new Dictionary<string, string>();
        }
        public string Content { get; set; }
        public string DescriptiveContent { get; set; }
        public IDictionary<string,string> ContextVariables { get; set; }
    }
}
