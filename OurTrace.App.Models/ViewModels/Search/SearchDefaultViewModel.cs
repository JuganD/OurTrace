using System;
using System.Collections.Generic;
using System.Text;

namespace OurTrace.App.Models.ViewModels.Search
{
    public class SearchDefaultViewModel
    {
        public int Count { get; set; }
        public string Query { get; set; }
        public ICollection<SearchResultViewModel> Values { get; set; }
    }
}
