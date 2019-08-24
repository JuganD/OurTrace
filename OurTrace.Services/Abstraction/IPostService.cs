using OurTrace.App.Models.InputModels.Posts;
using OurTrace.App.Models.InputModels.Share;
using OurTrace.App.Models.ViewModels.Post;
using OurTrace.Data.Identity.Models;
using OurTrace.Data.Models;
using System.Threading.Tasks;

namespace OurTrace.Services.Abstraction
{
    public interface IPostService
    {
        Task<bool> CreateNewPostAsync(string username, CreatePostInputModel model);
        Task<bool> LikePostAsync(string username, string postId);
        Task<bool> CommentPostAsync(string username, string postId, string content);
        Task<bool> LikeCommentAsync(string username, string commentId);
        Task<bool> SharePostAsync(string username, ShareInputModel model);
        Task<bool> IsUserCanPostToWallAsync(string username, string WallId);
        Task<bool> IsUserCanSeePostAsync(string username, string postId);
        Task<string> GetPostOwnerUsernameAsync(string postId);
        Task<PostViewModel> GetShareViewAsync(string postId);
    }
}
