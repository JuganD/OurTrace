using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OurTrace.App.Models.ViewModels.Group;
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

        public GroupService(OurTraceDbContext dbContext,
            IMapper automapper)
        {
            this.dbContext = dbContext;
            this.automapper = automapper;
            this.identityService = new IdentityService(dbContext);
        }

        public async Task CreateNewGroupAsync(string name, string ownerUsername)
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
                    await this.dbContext.SaveChangesAsync();
                }
            }
        }

        public async Task<IEnumerable<GroupWindowViewModel>> DiscoverGroupsAsync()
        {
            // TODO: figure out a better algoritm for finding groups
            var famousGroups = await dbContext.Groups
                .OrderByDescending(x => x.Members.Count)
                .Take(50)
                .ToListAsync();

            return automapper.Map<IEnumerable<GroupWindowViewModel>>(famousGroups);
        }

        public async Task<IEnumerable<GroupWindowViewModel>> GetUserGroupsAsync(string username)
        {
            var user = await identityService.GetUserByName(username)
                .Include(x=>x.Groups).SingleOrDefaultAsync();

            if (user != null)
            {
                List<Group> groups = user.Groups
                    .OrderByDescending(x => x.Members.Count)
                    .ToList();

                return automapper.Map<IEnumerable<GroupWindowViewModel>>(groups);
            }

            return null;
        }

        public async Task<bool> GroupExistAsync(string name)
        {
            return await dbContext.Groups
                .AnyAsync(x => x.Name == name ||
                               x.Url == StringifyGroupName(name));
        }
        private string StringifyGroupName(string name)
        {
            return new string(name.Where(x => char.IsLetter(x) || char.IsNumber(x)).ToArray());
        }
    }
}
