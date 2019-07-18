using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OurTrace.App.Models.InputModels.Posts;
using OurTrace.Data;
using OurTrace.Data.Identity.Models;
using OurTrace.Data.Models;
using OurTrace.Services.Abstraction;

namespace OurTrace.Services
{
    public class PostService : IPostService
    {
        private readonly OurTraceDbContext dbContext;
        private readonly IIdentityService identityService;
        private readonly IRelationsService relationsService;
        private readonly WallService wallService;

        public PostService(OurTraceDbContext dbContext,
            IIdentityService identityService,
            IRelationsService relationsService)
        {
            this.dbContext = dbContext;
            this.identityService = identityService;
            this.relationsService = relationsService;
            this.wallService = new WallService(dbContext);
        }

        public async Task<bool> CreateNewPostAsync(string username, CreatePostInputModel model)
        {
            if (await IsUserCanPostToWallAsync(username, model.Location))
            {
                // TODO: save the file somewhere and add mediaUrl
                var wall = await wallService.GetWallAsync(model.Location);

                IEnumerable<string> tags = new string[0];
                if (model.Tags != null && model.Tags.Length > 0)
                {
                    tags = model.Tags.Split(',').Select(x => x.Trim());
                }

                wall.Posts.Add(new Post()
                {
                    Location = wall,
                    User = await identityService.GetUserAsync(username),
                    Content = model.Content,
                    Tags = tags
                });
                await this.dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> IsUserCanPostToWallAsync(string username, string WallId)
        {
            var user = await identityService.GetUserAsync(username);
            var wall = await wallService.GetWallAsync(WallId);
            var wallOwnerId = await wallService.GetWallOwnerIdAsync(wall);

            if (wallOwnerId == null) return false;

            if (wallOwnerId == user.Id) return true;

            var wallUser = await identityService.GetUserByIdAsync(wallOwnerId);
            if (wallUser != null)
            {
                if (await relationsService.AreFriendsWithAsync(user.UserName, wallUser.UserName))
                    return true;
                else
                    return false;
            }
            else
            {
                var wallGroup = await this.dbContext.Groups.SingleOrDefaultAsync(x => x.Id == wallOwnerId);
                if (wallGroup.Members.Any(x => x.User == user))
                    return true;
                else
                    return false;
            }
        }

    }
}
