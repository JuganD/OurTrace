using Microsoft.EntityFrameworkCore;
using OurTrace.Data;
using OurTrace.Data.Models;
using OurTrace.Services.Helpers;
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

        internal IQueryable<Wall> GetWallWithIncludables(string wallId)
        {
            var query = this.dbContext.Walls
                .Include(this.dbContext.GetIncludePaths(typeof(Wall)))
                .Where(x=>x.Id==wallId);

            return query;
        }
        internal IQueryable<Wall> GetWallWithoutIncludables(string wallId)
        {
            return this.dbContext.Walls
                .Where(x => x.Id == wallId);
        }

        internal async Task<string> GetWallOwnerIdAsync(Wall wall)
        {
            var user = await this.dbContext.Users.SingleOrDefaultAsync(x => x.Wall == wall);
            if (user != null) return user.Id;

            var group = await this.dbContext.Groups.SingleOrDefaultAsync(x => x.Wall == wall);
            if (group != null) return group.Id;

            return null;
        }
        internal async Task<string> GetWallOwnerNameAsync(Wall wall)
        {
            var user = await this.dbContext.Users.SingleOrDefaultAsync(x => x.Wall == wall);
            if (user != null) return user.UserName;

            var group = await this.dbContext.Groups.SingleOrDefaultAsync(x => x.Wall == wall);
            if (group != null) return group.Name;

            return null;
        }
        internal async Task<Wall> GetUserWallAsync(string username)
        {
            return (await this.dbContext.Users
                .Include(x => x.Wall)
                .SingleOrDefaultAsync(x => x.UserName == username)).Wall;
        }

        internal async Task<Wall> GetGroupWallAsync(string name)
        {
            return (await this.dbContext.Groups
                .Include(x => x.Wall)
                .SingleOrDefaultAsync(x => x.Name == name)).Wall;
        }

        internal async Task<ICollection<Post>> GetPostsFromWallDescendingAsync(string wallId)
        {
            var wall = await GetWallWithIncludables(wallId)
                .SingleOrDefaultAsync();
            var result = wall.Posts
                .OrderByDescending(x => x.CreatedOn)
                .ToArray();
            foreach (var post in result)
            {
                // newest on the bottom
                post.Comments = post.Comments.OrderBy(x => x.CreatedOn).ToArray();
            }
            return result;
        }

        internal async Task<bool> IsWallBelongsToGroup(string wallId)
        {
            return await this.dbContext.Groups.AnyAsync(x => x.WallId == wallId);
        }
    }
}
