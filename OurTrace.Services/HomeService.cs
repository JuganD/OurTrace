using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OurTrace.App.Models;
using OurTrace.App.Models.ViewModels.Home;
using OurTrace.App.Models.ViewModels.Post;
using OurTrace.Data;
using OurTrace.Data.Identity.Models;
using OurTrace.Data.Models;
using OurTrace.Services.Abstraction;
using OurTrace.Services.Helpers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OurTrace.Services
{
    public class HomeService : IHomeService
    {
        private static readonly ConcurrentDictionary<string, NewsfeedDataModel> NewsfeedState =
            new ConcurrentDictionary<string, NewsfeedDataModel>();

        private readonly OurTraceDbContext dbContext;
        private readonly IMapper automapper;
        private readonly IAdvertService advertService;
        private readonly IRelationsService relationsService;
        private readonly IGroupService groupService;
        private readonly IdentityService identityService;
        private readonly WallService wallService;

        public HomeService(OurTraceDbContext dbContext,
            IMapper automapper,
            IAdvertService advertService,
            IRelationsService relationsService,
            IGroupService groupService)
        {
            this.dbContext = dbContext;
            this.automapper = automapper;
            this.advertService = advertService;
            this.relationsService = relationsService;
            this.groupService = groupService;
            this.identityService = new IdentityService(dbContext);
            this.wallService = new WallService(dbContext);
        }
        public async Task<NewsfeedViewModel> GetNewsfeedViewModelAsync(string userId)
        {
            var user = await this.identityService.GetUserById(userId)
                .Include(x => x.Followers)
                .Include(x => x.Following)
                .SingleOrDefaultAsync();

            if (user != null)
            {
                var viewModel = this.automapper.Map<NewsfeedViewModel>(user);
                viewModel.Posts = await GetPostsForNewsfeedAsync(userId, false);

                if (viewModel.Posts.Count > 0)
                {
                    // Because we want the adverts on the top of each page iteration
                    viewModel.Posts.Random().Adverts = await this.advertService.
                        GetRandomAdvertsAndSanctionThemAsync(5);
                }

                viewModel.MemberOfGroups = (await this.groupService
                            .GetUserGroupsAsync(user.UserName))
                        .Select(x => x.Name).ToList();

                viewModel.HotOffer = (await this.advertService.GetRandomAdvertsAndSanctionThemAsync(1)).FirstOrDefault();

                return viewModel;
            }

            return null;
        }
        public async Task<ICollection<PostViewModel>> GetPostsForNewsfeedAsync(string userId, bool overrideCounter)
        {
            var newsfeedState = await GetNewsfeedStateModelById(userId);
            if (newsfeedState.Model.Posts.Count - newsfeedState.Location > 20 || overrideCounter)
            {
                List<PostViewModel> resultPosts;
                if (!overrideCounter)
                {
                    newsfeedState.Model.Posts = newsfeedState.Model.Posts
                    .Skip(newsfeedState.Location)
                    .Take(20)
                    .ToList();

                    newsfeedState.Location += 20;
                }

                resultPosts = newsfeedState.Model.Posts.ToList();

                return resultPosts;
            }
            else
            {
                // Posts are either 0 or less than 20 which means we iterate again
                newsfeedState.Location = 0;
                var resultPosts = newsfeedState.Model.Posts
                    .ToList();
                resultPosts.ForEach(x => x.Adverts = null);
                newsfeedState.Model.Posts.Clear();

                // Don't look down! It's scary!

                // FRIENDS
                var friendsOfTarget = await this.relationsService.GetFriendsUsernamesAsync(newsfeedState.Username);
                var friendsPosts = friendsOfTarget
                    .Select(x => this.wallService.GetUserWallAsync(x).GetAwaiter().GetResult())
                    .SelectMany(x => this.wallService.GetWallWithIncludables(x.Id).Take(5))
                    .SelectMany(x => x.Posts
                             .Where(y => y.UserId != userId)
                             .OrderByDescending(y => y.CreatedOn))
                    .ToList();

                // FRIENDS END

                // FOLLOWERS
                var followingOfTargetAll = (await this.identityService
                    .GetUserByName(newsfeedState.Username)
                    .SingleOrDefaultAsync()).Following;

                var followingOfTargetSender = followingOfTargetAll
                    .Where(x => x.Sender.UserName != newsfeedState.Username)
                    .Select(x=>x.Sender)
                    .ToList();

                var followingOfTargetRecipient = followingOfTargetAll
                    .Where(x => x.Recipient.UserName != newsfeedState.Username)
                    .Select(x => x.Recipient)
                    .ToList();

                var followingOfTarget = new List<OurTraceUser>();
                followingOfTarget.AddRange(followingOfTargetRecipient);
                followingOfTarget.AddRange(followingOfTargetSender);
                followingOfTarget = followingOfTarget
                    .DistinctBy(x => x.UserName)
                    .ToList();

                var followingPosts = followingOfTarget
                    .SelectMany(x => x.Posts.Where(y => y.VisibilityType == PostVisibilityType.Public))
                    .OrderByDescending(y => y.CreatedOn)
                    .Select(y => automapper.Map<PostViewModel>(y))
                    .ToList();

                // FOLLOWERS END

                // GROUPS

                var groupsOfTarget = await this.dbContext.UserGroups
                    .Include(x => x.Group)
                        .ThenInclude(x => x.Wall)
                    .Where(x => x.User.Id == userId && x.ConfirmedMember == true)
                    .ToListAsync();


                var postsOfGroupsOfTarget = groupsOfTarget
                    .SelectMany(x => wallService.GetWallWithIncludables(x.Group.Wall.Id).Take(5))
                    .SelectMany(x => x.Posts
                            .Where(y => y.UserId != userId && y.VisibilityType == PostVisibilityType.Public)
                            .OrderByDescending(y => y.CreatedOn)
                            .Select(y => automapper.Map<PostViewModel>(y))
                            .Select(y =>
                            {
                                y.PostGroupName = this.wallService
                     .GetWallOwnerNameAsync(x).GetAwaiter().GetResult(); return y;
                            }))
                    .ToList();

                // GROUPS END

                newsfeedState.Model.Posts = resultPosts.Union(
                    automapper.Map<ICollection<PostViewModel>>(friendsPosts))
                    .Union(postsOfGroupsOfTarget)
                    .Union(followingPosts)
                    .DistinctBy(x => x.Id)
                    .ToList()
                    .Shuffle();


                if (newsfeedState.Model.Posts.Count < 20)
                {
                    return await GetPostsForNewsfeedAsync(userId, true);
                }

                return await GetPostsForNewsfeedAsync(userId, false);
            }
        }

        public async Task<string> GetUserIdFromName(string username)
        {
            return (await this.dbContext.Users
                .SingleOrDefaultAsync(x => x.UserName == username))
                .Id;
        }
        private async Task<NewsfeedDataModel> GetNewsfeedStateModelById(string userId)
        {
            NewsfeedDataModel userState = new NewsfeedDataModel();
            if (!NewsfeedState.ContainsKey(userId))
            {
                userState.Location = 0;
                userState.Model = new NewsfeedViewModel();
                userState.Username = await this.identityService.GetUserById(userId)
                    .Select(x => x.UserName)
                    .SingleOrDefaultAsync();

                NewsfeedState.TryAdd(userId, userState);
            }
            else
            {
                NewsfeedState.TryGetValue(userId, out userState);
            }
            return userState;
        }
    }
}
