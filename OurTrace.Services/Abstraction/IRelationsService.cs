using OurTrace.App.Models.ViewModels.Profile;
using OurTrace.App.Models.ViewModels.Settings;
using OurTrace.Data.Identity.Models;
using OurTrace.Data.Models;
using System.Collections.Generic;
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
        Task<ICollection<ProfileFriendSuggestionViewModel>> GetFriendsOfFriendsAsync(string username, int count);
        Task<ICollection<string>> GetFriendsUsernamesAsync(string username);
        Task<ICollection<SettingsFriendRequestViewModel>> GetPendingFriendRequestsAsync(string username);
    }
}
