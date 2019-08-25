using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using OurTrace.App.Models.ViewModels.Group;
using OurTrace.App.Models.ViewModels.Post;
using OurTrace.Data;
using OurTrace.Data.Models;
using OurTrace.Services.Abstraction;

namespace OurTrace.Services
{
    public class GroupService : IGroupService
    {
        private readonly OurTraceDbContext dbContext;
        private readonly IMapper automapper;
        private readonly IdentityService identityService;
        private readonly WallService wallService;

        public GroupService(OurTraceDbContext dbContext,
            IMapper automapper)
        {
            this.dbContext = dbContext;
            this.automapper = automapper;

            this.wallService = new WallService(dbContext);
            this.identityService = new IdentityService(dbContext);
        }

        public async Task<bool> CreateNewGroupAsync(string name, string ownerUsername)
        {
            var user = await identityService.GetUserByName(ownerUsername).SingleOrDefaultAsync();
            if (user != null)
            {
                if (!await GroupExistAsync(name))
                {
                    Group group = new Group()
                    {
                        Creator = user,
                        Name = name,
                        Url = StringifyGroupName(name)
                    };
                    this.dbContext.Groups.Add(group);
                    this.dbContext.UserGroups.Add(new UserGroup()
                    {
                        Group = group,
                        User = user,
                        ConfirmedMember = true
                    });
                    this.dbContext.GroupAdmins.Add(new GroupAdmin()
                    {
                        Group = group,
                        User = user,
                        AdminType = GroupAdminType.Admin
                    });
                    await this.dbContext.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public async Task<IEnumerable<GroupWindowViewModel>> DiscoverGroupsAsync(string username)
        {
            // TODO: figure out a better algoritm for finding groups
            var famousGroups = await dbContext.Groups
                .Where(x => !x.Members.Any(y => y.User.UserName == username && y.ConfirmedMember == true))
                .Include(x => x.Members)
                    .ThenInclude(x => x.User)
                .OrderByDescending(x => x.Members.Count)
                .Take(50)
                .ToListAsync();

            var viewModels = automapper.Map<IEnumerable<GroupWindowViewModel>>(famousGroups);
            var pendingGroups = famousGroups.Where(x => x.Members.Any(y => y.User.UserName == username && y.ConfirmedMember == false));
            foreach (var model in viewModels)
            {
                if (pendingGroups.Any(x => x.Name == model.Name))
                {
                    model.PendingJoin = true;
                }
            }
            return viewModels;
        }

        public async Task<IEnumerable<GroupWindowViewModel>> GetUserGroupsAsync(string username)
        {
            var user = await identityService.GetUserByName(username)
                .SingleOrDefaultAsync();


            if (user != null)
            {
                var groupsMemberOf = this.dbContext.Groups
                    .Where(x => x.Members.Any(y => y.User == user && y.ConfirmedMember == true))
                    .Include(x => x.Members);

                return automapper.Map<IEnumerable<GroupWindowViewModel>>(groupsMemberOf);
            }

            return null;
        }

        public async Task<GroupOpenViewModel> PrepareGroupForViewAsync(string name, string username)
        {
            var group = await GetGroup(name)
            .Include(x => x.Members)
                .ThenInclude(x => x.User)
            .SingleOrDefaultAsync();

            if (group != null)
            {
                // EF core currently can't be limited to include only 15
                group.Members = group.Members.Take(15).ToList();

                var groupViewModel = automapper.Map<GroupOpenViewModel>(group);

                groupViewModel.Posts = automapper.Map<ICollection<PostViewModel>>(await wallService.GetPostsFromWallDescendingAsync(groupViewModel.WallId));
                groupViewModel.JoinRequests = automapper.Map<ICollection<GroupMemberViewModel>>(group.Members.Where(x => x.ConfirmedMember == false));

                groupViewModel.IsUserMemberOfGroup = await
                    IsUserMemberOfGroupAsync(name, username);

                groupViewModel.IsUserConfirmed = await
                    IsUserConfirmedMemberAsync(name, username);

                groupViewModel.IsAdministrator = await
                    IsUserHaveRoleAsync(name, username, "Admin");

                groupViewModel.GroupRank = this.dbContext.Groups
                    .OrderByDescending(x => x.Members.Count)
                    .ThenBy(x => x.CreatedOn)
                    .IndexOf(group) + 1;

                if (!groupViewModel.IsAdministrator)
                {
                    // Because administrator has more rights anyway, so no need to check them both
                    // if user is already confirmed administrator
                    groupViewModel.IsModerator = await
                        IsUserHaveRoleAsync(name, username, "Moderator");
                }

                return groupViewModel;
            }

            return null;
        }

        public async Task<bool> IsUserMemberOfGroupAsync(string groupname, string username)
        {
            var user = await identityService.GetUserByName(username)
                .SingleOrDefaultAsync();
            var group = await GetGroup(groupname)
                .Include(x => x.Members)
                    .ThenInclude(x => x.User)
                .SingleOrDefaultAsync();

            if (user != null && group != null)
            {
                return group.Members.Any(x => x.User == user);
            }

            return false;
        }
        public async Task<bool> IsUserConfirmedMemberAsync(string groupname, string username)
        {
            var user = await identityService.GetUserByName(username)
                .SingleOrDefaultAsync();

            var group = await GetGroup(groupname)
                .SingleOrDefaultAsync();

            if (user != null && group != null)
            {
                return this.dbContext.UserGroups.Any(x =>
                    x.Group == group &&
                    x.User == user &&
                    x.ConfirmedMember == true);
            }

            return false;
        }

        public async Task<bool> IsUserHaveRoleAsync(string groupname, string username, string roleName)
        {
            GroupAdminType roleType = GroupAdminType.Admin;
            if (Enum.TryParse(roleName, true, out roleType))
            {
                var user = await identityService.GetUserByName(username)
                    .Include(x => x.Groups)
                    .SingleOrDefaultAsync();

                var group = user.Groups.SingleOrDefault(x => x.Name == groupname);

                if (user != null && group != null)
                {
                    return this.dbContext.GroupAdmins.Any(x =>
                        x.Group == group &&
                        x.User == user &&
                        x.AdminType == roleType);
                }
            }
            return false;
        }


        public async Task<bool> JoinGroupAsync(string groupname, string username)
        {
            var user = await identityService.GetUserByName(username)
                .SingleOrDefaultAsync();
            var group = await GetGroup(groupname)
                .SingleOrDefaultAsync();

            if (user != null && group != null)
            {
                this.dbContext.UserGroups.Add(new UserGroup()
                {
                    User = user,
                    Group = group
                });
                await this.dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> AcceptMemberAsync(string groupname, string username)
        {
            var user = await identityService.GetUserByName(username)
                .SingleOrDefaultAsync();
            var group = await GetGroup(groupname)
                .SingleOrDefaultAsync();

            if (user != null && group != null)
            {
                var userGroup = await this.dbContext.UserGroups
                    .SingleOrDefaultAsync(x => x.Group == group && x.User == user);

                userGroup.ConfirmedMember = true;

                await this.dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
        public async Task<bool> KickMemberAsync(string groupname, string username)
        {
            var user = await identityService.GetUserByName(username)
                .SingleOrDefaultAsync();
            var group = await GetGroup(groupname)
                .SingleOrDefaultAsync();

            if (user != null && group != null)
            {
                var userGroup = await this.dbContext.UserGroups
                    .SingleOrDefaultAsync(x => x.Group == group &&
                                          x.User == user &&
                                          x.ConfirmedMember == true);
                if (userGroup != null)
                {
                    this.dbContext.UserGroups.Remove(userGroup);
                    await this.dbContext.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        public async Task<IEnumerable<GroupMemberViewModel>> GetGroupMembersAsync(string groupname)
        {
            var userGroups = await GetGroup(groupname)
                .SelectMany(x => x.Members)
                    .Include(x => x.User)
                .Where(x => x.ConfirmedMember == true)
                .ToListAsync();

            return automapper.Map<IEnumerable<GroupMemberViewModel>>(userGroups);
        }
        public async Task<string> GetGroupOwnerAsync(string groupname)
        {
            return (await this.dbContext.Groups
                .Include(x => x.Creator)
                .SingleOrDefaultAsync(x => x.Name == groupname)).Creator.UserName;
        }
        public async Task<bool> IsUserHaveAnyAdministratorRightsAsync(string groupname, string username)
        {
            var user = await identityService.GetUserByName(username)
                .SingleOrDefaultAsync();
            var group = await GetGroup(groupname)
                .SingleOrDefaultAsync();

            if (user != null && group != null)
            {
                var groupAdmin = await this.dbContext.GroupAdmins
                    .SingleOrDefaultAsync(x => x.User == user && x.Group == group);
                if (groupAdmin != null)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> GroupExistAsync(string name)
        {
            return await GetGroup(name).CountAsync() == 1;
        }
        private string StringifyGroupName(string name)
        {
            return new string(name.Where(x => char.IsLetter(x) || char.IsNumber(x)).ToArray());
        }
        private IQueryable<Group> GetGroup(string name)
        {
            return this.dbContext.Groups
                .Where(x => x.Name == name ||
                            x.Url == StringifyGroupName(name));
        }
    }
}
