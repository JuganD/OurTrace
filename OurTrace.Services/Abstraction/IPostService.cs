using OurTrace.App.Models.InputModels.Posts;
using OurTrace.Data.Identity.Models;
using OurTrace.Data.Models;
using System.Threading.Tasks;

namespace OurTrace.Services.Abstraction
{
    public interface IPostService
    {
        Task<bool> CreateNewPostAsync(string username, CreatePostInputModel model);
        Task<bool> IsUserCanPostToWallAsync(string username, string WallId);
    }
}
