using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OurTrace.Data;
using OurTrace.Data.Identity.Models;
using OurTrace.Data.Models;
using OurTrace.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OurTrace.Services
{
    public class UsersService : IUsersService
    {
        private readonly OurTraceDbContext dbContext;

        public UsersService(OurTraceDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public OurTraceUser GetNewUser(string username, string email, string fullname, DateTime? birthDate)
        {
            var wall = new Wall();

            var user = new OurTraceUser
            {
                UserName = username,
                Email = email,
                FullName = fullname,
                BirthDate = birthDate,
                Wall = wall
            };

            return user;
        }

        public async Task<List<OurTraceUser>> GetAllUsersAsync()
        {
            return await dbContext.Users.ToListAsync();
        }

        public async Task<OurTraceUser> GetUserAsync(string username)
        {
            return await AttachRequiredInclusionsToUser(this.dbContext.Users)
                .SingleOrDefaultAsync(x => x.UserName == username);
        }
        public async Task<OurTraceUser> GetUserByIdAsync(string id)
        {
            return await AttachRequiredInclusionsToUser(this.dbContext.Users)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddFollowerAsync(OurTraceUser sender, OurTraceUser recipient)
        {
            if (sender.Id != recipient.Id)
            {
                await this.dbContext.Follows.AddAsync(new Follow()
                {
                    Sender = sender,
                    Recipient = recipient
                });
                await this.dbContext.SaveChangesAsync();
            }
        }
        public async Task<bool> CheckFollowExistsAsync(OurTraceUser sender, OurTraceUser recipient)
        {
            int follows = await this.dbContext.Follows.CountAsync(x => x.Sender == sender && x.Recipient == recipient);
            return follows > 0;
        }
        private IQueryable<OurTraceUser> AttachRequiredInclusionsToUser(IQueryable<OurTraceUser> query)
        {
            return query
                .Include(x => x.Followers)
                .Include(x => x.Following)
                .Include(x => x.Wall)
                    .ThenInclude(x => x.Posts)
                .Include(x => x.Comments);
        }

        public async Task AddLikeAsync(string postId, string userId)
        {
            var user = await GetUserByIdAsync(userId);
            var post = await this.dbContext.Posts.SingleOrDefaultAsync(x => x.Id == postId);

            if (!post.Likes.Any(x=>x.User == user && x.Post == post))
            {
                post.Likes.Add(new PostLike()
                {
                    Post = post,
                    User = user
                });
                await this.dbContext.SaveChangesAsync();
            }
        }
    }
}
