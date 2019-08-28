using OurTrace.App.Models.ViewModels.Home;
using OurTrace.App.Models.ViewModels.Post;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OurTrace.Services.Abstraction
{
    public interface IHomeService
    {
        Task<string> GetUserIdFromName(string username);
        Task<NewsfeedViewModel> GetNewsfeedViewModelAsync(string userId);
        Task<ICollection<PostViewModel>> GetPostsForNewsfeedAsync(string userId, bool overrideCounter);
    }
}
