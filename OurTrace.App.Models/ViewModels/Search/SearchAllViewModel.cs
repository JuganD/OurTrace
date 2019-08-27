using System;
using System.Collections.Generic;
using System.Text;

namespace OurTrace.App.Models.ViewModels.Search
{
    public class SearchAllViewModel
    {
        public string Query { get; set; }
        public ICollection<SearchResultViewModel> Users { get; set; }
        public ICollection<SearchResultViewModel> Groups { get; set; }
        public ICollection<SearchResultViewModel> Posts { get; set; }
        public ICollection<SearchResultViewModel> Comments { get; set; }
    }
}
