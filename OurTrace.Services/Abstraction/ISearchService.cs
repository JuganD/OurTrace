using OurTrace.App.Models.ViewModels.Search;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OurTrace.Services.Abstraction
{
    public interface ISearchService
    {
        Task<ICollection<SearchResultViewModel>> SearchForPeopleAsync(string query);
        Task<ICollection<SearchResultViewModel>> SearchForGroupsAsync(string query);
        Task<ICollection<SearchResultViewModel>> SearchForPostsAsync(string query);
        Task<ICollection<SearchResultViewModel>> SearchForCommentsAsync(string query);
        Task<IDictionary<SearchResultType, ICollection<SearchResultViewModel>>> SearchForEverythingAsync(string query);
    }
}
