using OurTrace.App.Models.ViewModels.Group;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OurTrace.Services.Abstraction
{
    public interface IGroupService
    {
        Task CreateNewGroupAsync(string name, string ownerUsername);
        Task<bool> GroupExistAsync(string name);
        Task<IEnumerable<GroupWindowViewModel>> DiscoverGroupsAsync();
        Task<IEnumerable<GroupWindowViewModel>> GetUserGroupsAsync(string username);
    }
}
