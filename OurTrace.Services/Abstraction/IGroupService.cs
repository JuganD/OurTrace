using OurTrace.App.Models.ViewModels.Group;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OurTrace.Services.Abstraction
{
    public interface IGroupService
    {
        Task<bool> CreateNewGroupAsync(string name, string ownerUsername);
        Task<bool> GroupExistAsync(string name);
        Task<bool> IsUserMemberOfGroupAsync(string groupname, string username);
        Task<bool> IsUserConfirmedMemberAsync(string groupname, string username);
        Task<bool> IsUserHaveRoleAsync(string groupname, string username, string roleName);
        Task<bool> IsUserHaveAnyAdministratorRightsAsync(string groupname, string username);
        Task<bool> KickMemberAsync(string groupname, string username);
        Task<bool> JoinGroupAsync(string groupname, string username);
        Task<bool> AcceptMemberAsync(string groupname, string username);
        Task<IEnumerable<GroupWindowViewModel>> DiscoverGroupsAsync(string username);
        Task<IEnumerable<GroupWindowViewModel>> GetUserGroupsAsync(string username);
        Task<IEnumerable<GroupMemberViewModel>> GetGroupMembersAsync(string groupname);
        Task<GroupOpenViewModel> PrepareGroupForViewAsync(string name, string username);
    }
}
