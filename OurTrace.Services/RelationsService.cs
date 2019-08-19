using Microsoft.EntityFrameworkCore;
using OurTrace.Data;
using OurTrace.Data.Identity.Models;
using OurTrace.Data.Models;
using OurTrace.Services.Abstraction;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OurTrace.Services
{
    public class RelationsService : IRelationsService
    {
        private readonly OurTraceDbContext dbContext;
        private readonly IdentityService identityService;

        public RelationsService(OurTraceDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.identityService = new IdentityService(dbContext);
        }
        public async Task<bool> AreFriendsWithAsync(string firstUser, string secondUser)
        {
            if (await this.dbContext.Friendships.AnyAsync(x => x.Sender.UserName == firstUser && x.Recipient.UserName == secondUser && x.AcceptedOn != null) ||
                await this.dbContext.Friendships.AnyAsync(x => x.Recipient.UserName == firstUser && x.Sender.UserName == secondUser && x.AcceptedOn != null))
                return true;

            return false;
        }
        public async Task<bool> IsFollowingAsync(string firstUsername, string secondUsername)
        {
            int follows = await this.dbContext.Follows.CountAsync(x => x.Sender.UserName == firstUsername && x.Recipient.UserName == secondUsername);
            return follows > 0;
        }
        public async Task<bool> IsFollowingAsync(OurTraceUser firstUser, OurTraceUser secondUser)
        {
            int follows = await this.dbContext.Follows.CountAsync(x => x.Sender == firstUser && x.Recipient == secondUser);
            return follows > 0;
        }
        public async Task AddFriendshipAsync(string senderUsername, string receiverUsername)
        {
            if (await AreFriendsWithAsync(senderUsername, receiverUsername)) return;

            var friendship = await GetFriendshipAsync(senderUsername, receiverUsername);
            if (friendship != null)
            {
                friendship.AcceptedOn = DateTime.UtcNow;
                await this.dbContext.SaveChangesAsync();
                return;
            }

            var sender = await identityService.GetUserByName(senderUsername).SingleOrDefaultAsync();
            var receiver = await identityService.GetUserByName(receiverUsername).SingleOrDefaultAsync();


            if (sender != null && receiver != null)
            {
                if (friendship != null)
                {
                    friendship.AcceptedOn = DateTime.UtcNow;
                }
                else
                {
                    await this.dbContext.Friendships.AddAsync(new Friendship()
                    {
                        Sender = sender,
                        Recipient = receiver
                    });
                }
                await this.dbContext.SaveChangesAsync();
            }
        }
        public async Task RevokeFriendshipAsync(string senderUsername, string receiverUsername)
        {
            var friendship = await GetFriendshipAsync(senderUsername, receiverUsername);

            if (friendship != null)
            {
                this.dbContext.Friendships.Remove(friendship);

                await this.dbContext.SaveChangesAsync();
            }
        }


        public async Task AddLikeAsync(string postId, string userId)
        {
            var user = await identityService.GetUserById(userId).SingleOrDefaultAsync();
            var post = await this.dbContext.Posts.SingleOrDefaultAsync(x => x.Id == postId);

            if (!post.Likes.Any(x => x.User == user && x.Post == post))
            {
                post.Likes.Add(new PostLike()
                {
                    Post = post,
                    User = user
                });
                await this.dbContext.SaveChangesAsync();
            }
        }


        public async Task AddFollowerAsync(string senderUsername, string receiverUsername)
        {
            if (senderUsername != receiverUsername)
            {
                if (!await IsFollowingAsync(senderUsername, receiverUsername))
                {
                    await this.dbContext.Follows.AddAsync(new Follow()
                    {
                        Sender = await identityService.GetUserByName(senderUsername).SingleOrDefaultAsync(),
                        Recipient = await identityService.GetUserByName(receiverUsername).SingleOrDefaultAsync()
                    });
                    await this.dbContext.SaveChangesAsync();
                }
            }
        }
        public async Task RevokeFollowingAsync(string senderUsername, string receiverUsername)
        {
            var follow = await GetFollowAsync(senderUsername, receiverUsername);

            if (follow != null)
            {
                this.dbContext.Follows.Remove(follow);

                await this.dbContext.SaveChangesAsync();
            }
        }

        public async Task<Friendship> GetFriendshipAsync(string firstUsername, string secondUsername)
        {
            var friendship = await this.dbContext.Friendships.SingleOrDefaultAsync(x => x.Sender.UserName == firstUsername && x.Recipient.UserName == secondUsername);
            if (friendship != null) return friendship;

            friendship = await this.dbContext.Friendships.SingleOrDefaultAsync(x => x.Recipient.UserName == firstUsername && x.Sender.UserName == secondUsername);

            return friendship;
        }

        public async Task<Follow> GetFollowAsync(string firstUsername, string secondUsername)
        {
            var follow = await this.dbContext.Follows.SingleOrDefaultAsync(x => x.Sender.UserName == firstUsername && x.Recipient.UserName == secondUsername);
            if (follow != null) return follow;

            follow = await this.dbContext.Follows.SingleOrDefaultAsync(x => x.Recipient.UserName == firstUsername && x.Sender.UserName == secondUsername);

            return follow;
        }
    }
}
