using OurTrace.App.Models.ViewModels.Search;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OurTrace.Services.Abstraction
{
    public interface ISearchService
    {
        Task<SearchDefaultViewModel> SearchForUsersAsync(string query);
        Task<SearchDefaultViewModel> SearchForGroupsAsync(string query);
        Task<SearchDefaultViewModel> SearchForPostsAsync(string query, string username);
        Task<SearchDefaultViewModel> SearchForCommentsAsync(string query, string username);
        Task<SearchAllViewModel> SearchForEverythingAsync(string query, string username);
    }
}
