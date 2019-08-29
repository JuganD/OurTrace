using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OurTrace.App.Models.ViewModels.Profile;
using OurTrace.App.Models.ViewModels.Settings;
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
    public class RelationsService : IRelationsService
    {
        private readonly OurTraceDbContext dbContext;
        private readonly IMapper automapper;
        private readonly IdentityService identityService;

        public RelationsService(OurTraceDbContext dbContext, IMapper automapper)
        {
            this.dbContext = dbContext;
            this.automapper = automapper;
            this.identityService = new IdentityService(dbContext);
        }
        public async Task<bool> AreFriendsWithAsync(string firstUser, string secondUser)
        {
            if (firstUser == secondUser) return true;

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

        public async Task<ICollection<ProfileFriendSuggestionViewModel>> GetFriendsOfFriendsAsync(string username, int count)
        {
            string userId = (await identityService.GetUserByName(username)
                .SingleOrDefaultAsync()).Id;

            var friendShips = await this.dbContext.Friendships
                .Where(x => x.AcceptedOn != null &&
                           (x.Recipient.Id == userId || x.Sender.Id == userId))
                .ToListAsync();

            var result = new List<ProfileFriendSuggestionViewModel>();
            if (friendShips != null && friendShips.Count > 0)
            {
                int friendsCount = friendShips.Count;
                if (friendsCount > 0)
                {
                    Random rand = new Random();
                    int currentFriendshipNumber = -1;
                    int currentRandomFriendNumber = 0;

                    for (int i = 0; i < count; i++)
                    {
                        currentFriendshipNumber = friendsCount - 1 > currentFriendshipNumber ? currentFriendshipNumber + 1 : -1;
                        if (currentFriendshipNumber == -1) break;
                        var currentFriendship = friendShips[currentFriendshipNumber];

                        var currentFriendId = currentFriendship.Sender.Id == userId ? currentFriendship.Recipient.Id : currentFriendship.Sender.Id;

                        var randomFriendQuery = this.dbContext.Friendships
                            .Where(x => (x.Sender.Id == currentFriendId || x.Recipient.Id == currentFriendId) &&
                                       x.Sender.Id != userId && x.Recipient.Id != userId);

                        if (randomFriendQuery.Count() == 0) continue;

                        currentRandomFriendNumber = rand.Next(0, randomFriendQuery.Count() - 1);

                        var randomFriendship = await randomFriendQuery
                                                .Include(x => x.Recipient)
                                                .Include(x => x.Sender)
                                                .Skip(currentRandomFriendNumber)
                                                .FirstOrDefaultAsync();

                        var randomFriend = randomFriendship.Sender.Id == currentFriendId ? randomFriendship.Recipient : randomFriendship.Sender;
                        if (await AreFriendsWithAsync(username, randomFriend.UserName)) continue;
                        if (result.Any(x => x.Username == randomFriend.UserName)) continue;

                        var viewmodel = automapper.Map<ProfileFriendSuggestionViewModel>(randomFriend);
                        viewmodel.HisFriendUsername = randomFriendship.RecipientId == randomFriend.Id ? randomFriendship.Sender.UserName : randomFriendship.Recipient.UserName;

                        result.Add(viewmodel);
                    }
                }

            }

            return result;
        }

        public async Task<ICollection<SettingsFriendRequestViewModel>> GetPendingFriendRequestsAsync(string username)
        {
            var friendshipRequests = await this.dbContext.Friendships
                .Where(x => x.Recipient.UserName == username && x.AcceptedOn == null)
                .Include(x => x.Sender)
                .Select(x => x.Sender)
                .ToListAsync();

            return automapper.Map<List<SettingsFriendRequestViewModel>>(friendshipRequests);
        }

        // This service only
        private async Task<Friendship> GetFriendshipAsync(string firstUsername, string secondUsername)
        {
            var friendship = await this.dbContext.Friendships.SingleOrDefaultAsync(x => x.Sender.UserName == firstUsername && x.Recipient.UserName == secondUsername);
            if (friendship != null) return friendship;

            friendship = await this.dbContext.Friendships.SingleOrDefaultAsync(x => x.Recipient.UserName == firstUsername && x.Sender.UserName == secondUsername);

            return friendship;
        }

        // This service only
        private async Task<Follow> GetFollowAsync(string firstUsername, string secondUsername)
        {
            var follow = await this.dbContext.Follows.SingleOrDefaultAsync(x => x.Sender.UserName == firstUsername && x.Recipient.UserName == secondUsername);
            if (follow != null) return follow;

            follow = await this.dbContext.Follows.SingleOrDefaultAsync(x => x.Recipient.UserName == firstUsername && x.Sender.UserName == secondUsername);

            return follow;
        }

        public async Task<ICollection<string>> GetFriendsUsernamesAsync(string username)
        {
            var sentFriendships = await this.dbContext.Friendships
                .Where(x => x.AcceptedOn != null &&
                           (x.Sender.UserName == username))
                .Select(x=>x.Recipient.UserName)
                .ToListAsync();

            var receivedFriendships = await this.dbContext.Friendships
                .Where(x => x.AcceptedOn != null &&
                           (x.Recipient.UserName == username))
                .Select(x=>x.Sender.UserName)
                .ToListAsync();

            return sentFriendships.Union(receivedFriendships).ToList();
        }
    }
}
