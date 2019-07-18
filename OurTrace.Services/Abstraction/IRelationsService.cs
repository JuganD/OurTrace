using OurTrace.App.Models.ViewModels.Profile;
using OurTrace.Data.Identity.Models;
using OurTrace.Data.Models;
using System.Threading.Tasks;

namespace OurTrace.Services.Abstraction
{
    public interface IRelationsService
    {
        Task<bool> IsFollowingAsync(string firstUsername, string secondUsername);
        Task<bool> IsFollowingAsync(OurTraceUser firstUser, OurTraceUser secondUser);
        Task<bool> AreFriendsWithAsync(string firstUsername, string secondUsername);
        Task AddFollowerAsync(string senderUsername, string receiverUsername);
        Task AddLikeAsync(string postId, string userId);
        Task AddFriendshipAsync(string senderUsername, string receiverUsername);
        Task RevokeFriendshipAsync(string senderUsername, string receiverUsername);
        Task RevokeFollowingAsync(string senderUsername, string receiverUsername);

        Task<Friendship> GetFriendshipAsync(string firstUsername, string secondUsername);
        Task<Follow> GetFollowAsync(string firstUsername, string secondUsername);

        Task PrepareUserProfileForViewAsync(ProfileViewModel model, string actualUserName, OurTraceUser visitingUser);
    }
}
