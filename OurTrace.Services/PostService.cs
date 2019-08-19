using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GeekLearning.Storage;
using GeekLearning.Storage.Internal;
using Microsoft.EntityFrameworkCore;
using OurTrace.App.Models.InputModels.Posts;
using OurTrace.Data;
using OurTrace.Data.Models;
using OurTrace.Services.Abstraction;

namespace OurTrace.Services
{
    public class PostService : IPostService
    {
        private readonly OurTraceDbContext dbContext;
        private readonly IRelationsService relationsService;
        private readonly IMapper automapper;
        private readonly IStore fileStore;
        private readonly WallService wallService;
        private readonly IdentityService identityService;

        public PostService(OurTraceDbContext dbContext,
            IRelationsService relationsService,
            IStorageFactory storageFactory,
            IMapper automapper)
        {
            this.dbContext = dbContext;
            this.relationsService = relationsService;
            this.automapper = automapper;
            this.wallService = new WallService(dbContext);
            this.identityService = new IdentityService(dbContext);
            this.fileStore = storageFactory.GetStore("LocalFileStorage");
        }

        public async Task<bool> CreateNewPostAsync(string username, CreatePostInputModel model)
        {
            if (await IsUserCanPostToWallAsync(username, model.Location))
            {
                var wall = await wallService.GetWallAsync(model.Location);

                IEnumerable<string> tags = new string[0];
                if (model.Tags != null && model.Tags.Length > 0)
                {
                    tags = model.Tags.Split(',').Select(x => x.Trim());
                }

                Post post = automapper.Map<Post>(model);
                post.Location = wall;
                post.User = await identityService.GetUserByName(username).SingleOrDefaultAsync();
                post.Tags = tags;


                // FILE SAVING PROCEDURE
                if (model.MediaFile != null && model.MediaFile.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        model.MediaFile.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        await fileStore.SaveAsync(fileBytes, new PrivateFileReference(Path.Combine(username, post.Id)), "image/jpeg");
                    }
                    post.IsImageOnFileSystem = true;
                }
                else if (model.ExternalMediaUrl != null)
                {
                    post.MediaUrl = model.ExternalMediaUrl;
                }
                // logic separator



                wall.Posts.Add(post);
                await this.dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> IsUserCanPostToWallAsync(string username, string WallId)
        {
            var user = await identityService.GetUserByName(username)
                .SingleOrDefaultAsync();

            var wall = await wallService.GetWallAsync(WallId);
            var wallOwnerId = await wallService.GetWallOwnerIdAsync(wall);

            if (wallOwnerId == null) return false;

            if (wallOwnerId == user.Id) return true;

            var wallUser = await identityService.GetUserById(wallOwnerId)
                .SingleOrDefaultAsync();

            if (wallUser != null)
            {
                if (await relationsService.AreFriendsWithAsync(user.UserName, wallUser.UserName))
                    return true;
                else
                    return false;
            }
            else
            {
                var wallGroup = await this.dbContext.Groups
                    .Include(x=>x.Members)
                    .SingleOrDefaultAsync(x => x.Id == wallOwnerId);
                if (wallGroup.Members.Any(x => x.User == user))
                    return true;
                else
                    return false;
            }
        }
        public async Task<bool> IsUserCanSeePostAsync(string username, string postId)
        {
            var user = await identityService.GetUserByName(username).SingleOrDefaultAsync();
            var post = await this.dbContext.Posts
                .Include(x => x.User)
                .SingleOrDefaultAsync(x => x.Id == postId);
            var postOwner = post.User.UserName;

            if (user == null || post == null) return false;

            var postVisibility = post.VisibilityType;

            switch (postVisibility)
            {
                case PostVisibilityType.Public:
                    return true;
                case PostVisibilityType.FriendsOnly:
                    if (username == postOwner || await relationsService.AreFriendsWithAsync(username, postOwner))
                    {
                        return true;
                    }
                    return false;
                case PostVisibilityType.Private:
                    if (username == postOwner)
                    {
                        return true;
                    }
                    return false;
            }

            return false;
        }
    }
}
