using Microsoft.EntityFrameworkCore;
using OurTrace.Data;
using OurTrace.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OurTrace.Services
{
    // Why internal? Well, because I want to reuse those methods only in the scope of the
    // services. This way I can return database models without worrying about the application
    // getting them. Walls are not used in the view context of the app, they are only needed here
    // as localizator for the data.
    internal class WallService
    {
        private readonly OurTraceDbContext dbContext;

        public WallService(OurTraceDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        internal async Task<Wall> GetWallAsync(string wallId)
        {
            return await this.dbContext.Walls
                .Include(x=>x.Posts)
                    .ThenInclude(x=>x.Likes)
                    .ThenInclude(x=>x.User)
                    .ThenInclude(x=>x.Comments)
                        .ThenInclude(x=>x.User)
                .SingleOrDefaultAsync(x => x.Id == wallId);
        }

        internal async Task<string> GetWallOwnerIdAsync(Wall wall)
        {
            var user = await this.dbContext.Users.SingleOrDefaultAsync(x => x.Wall == wall);
            if (user != null) return user.Id;

            var group = await this.dbContext.Groups.SingleOrDefaultAsync(x => x.Wall == wall);
            if (group != null) return group.Id;

            return null;
        }

        internal async Task<ICollection<Post>> GetPostsFromWallDescendingAsync(string wallId)
        {
            var wall = await GetWallAsync(wallId);
            return wall.Posts
                .OrderByDescending(x => x.CreatedOn)
                .ToArray();
        }

        internal async Task<bool> IsWallBelongsToGroup(string wallId)
        {
            return await this.dbContext.Groups.AnyAsync(x => x.WallId == wallId);
        }
    }
}
