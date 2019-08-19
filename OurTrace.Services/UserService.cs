using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OurTrace.App.Models.ViewModels.Post;
using OurTrace.App.Models.ViewModels.Profile;
using OurTrace.Data;
using OurTrace.Data.Identity.Models;
using OurTrace.Data.Models;
using OurTrace.Services.Abstraction;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OurTrace.Services
{
    public class UserService : IUserService
    {
        private readonly IdentityService identityService;
        private readonly IRelationsService relationsService;
        private readonly IMapper mapper;
        private readonly WallService wallService;

        public UserService(OurTraceDbContext dbContext,
            IMapper mapper,
            IRelationsService relationsService)
        {
            this.mapper = mapper;
            this.relationsService = relationsService;

            this.wallService = new WallService(dbContext);
            this.identityService = new IdentityService(dbContext);
        }

        public async Task<ProfileViewModel> PrepareUserProfileForViewAsync(string actualUserName, string visitingUserName)
        {
            var visitingUser = await IncludeAllProfileDetails(identityService.GetUserByName(visitingUserName))
                .SingleOrDefaultAsync();
            ProfileViewModel model = mapper.Map<ProfileViewModel>(visitingUser);

            model.Posts = mapper.Map<ICollection<PostViewModel>>(await wallService.GetPostsFromWallDescendingAsync(model.WallId));

            if (actualUserName != visitingUser.UserName)
            {
                var actualUser = await IncludeFriendship(identityService.GetUserByName(actualUserName))
                    .SingleOrDefaultAsync();

                if (await relationsService.AreFriendsWithAsync(actualUserName, visitingUser.UserName))
                {
                    model.Posts = model.Posts
                        .Where(x => x.VisibilityType == PostVisibilityType.FriendsOnly ||
                                    x.VisibilityType == PostVisibilityType.Public)
                        .ToList();
                    model.AreFriends = true;
                }
                else
                {
                    model.Posts = model.Posts
                        .Where(x => x.VisibilityType == PostVisibilityType.Public)
                        .ToList();

                    if (actualUser.SentFriendships.Any(x =>
                        x.Recipient == visitingUser && x.AcceptedOn == null))
                    {
                        model.PendingFriendship = true;
                    }
                }

                if (await relationsService.IsFollowingAsync(actualUser, visitingUser))
                {
                    model.IsFollowing = true;
                }
            }
            else
            {
                model.IsHimself = true;
            }

            return model;
        }
        private IQueryable<OurTraceUser> IncludeFriendship(IQueryable<OurTraceUser> query)
        {
            return query
                .Include(x => x.SentFriendships)
                    .ThenInclude(x => x.Sender)
                 .Include(x => x.SentFriendships)
                    .ThenInclude(x => x.Recipient)
                .Include(x => x.ReceivedFriendships)
                    .ThenInclude(x => x.Sender)
                .Include(x => x.ReceivedFriendships)
                    .ThenInclude(x => x.Recipient);
        }
        private IQueryable<OurTraceUser> IncludeAllProfileDetails(IQueryable<OurTraceUser> query)
        {
            return query
                .Include(x => x.Followers)
                .Include(x => x.Following)
                .Include(x => x.Wall)
                    .ThenInclude(x => x.Posts)
                .Include(x => x.SentFriendships)
                    .ThenInclude(x => x.Sender)
                 .Include(x => x.SentFriendships)
                    .ThenInclude(x => x.Recipient)
                .Include(x => x.ReceivedFriendships)
                    .ThenInclude(x => x.Sender)
                .Include(x => x.ReceivedFriendships)
                    .ThenInclude(x => x.Recipient)
                .Include(x => x.Comments);
        }
    }
}
