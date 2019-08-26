using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GeekLearning.Storage;
using GeekLearning.Storage.Internal;
using Microsoft.EntityFrameworkCore;
using OurTrace.App.Models.InputModels.Posts;
using OurTrace.App.Models.InputModels.Share;
using OurTrace.App.Models.ViewModels.Post;
using OurTrace.Data;
using OurTrace.Data.Models;
using OurTrace.Services.Abstraction;
using OurTrace.Services.Helpers;

namespace OurTrace.Services
{
    public class PostService : IPostService
    {
        private readonly OurTraceDbContext dbContext;
        private readonly IRelationsService relationsService;
        private readonly IGroupService groupService;
        private readonly IFileService fileService;
        private readonly IMapper automapper;
        private readonly WallService wallService;
        private readonly IdentityService identityService;

        public PostService(OurTraceDbContext dbContext,
            IRelationsService relationsService,
            IGroupService groupService,
            IFileService fileService,
            IMapper automapper)
        {
            this.dbContext = dbContext;
            this.relationsService = relationsService;
            this.groupService = groupService;
            this.fileService = fileService;
            this.automapper = automapper;
            this.wallService = new WallService(dbContext);
            this.identityService = new IdentityService(dbContext);
        }

        public async Task<bool> CreateNewPostAsync(string username, CreatePostInputModel model)
        {
            if (await IsUserCanPostToWallAsync(username, model.Location))
            {
                var wall = await wallService.GetWallWithIncludables(model.Location);

                var tags = GetPostTags(model.Tags);

                Post post = automapper.Map<Post>(model);
                post.Location = wall;
                post.User = await identityService.GetUserByName(username).SingleOrDefaultAsync();
                post.Tags = tags;

                if (await wallService.IsWallBelongsToGroup(model.Location))
                {
                    post.VisibilityType = PostVisibilityType.Public;
                }

                // FILE SAVING PROCEDURE
                if (model.MediaFile != null && model.MediaFile.Length > 0)
                {
                    await this.fileService.SaveImageAsync(model.MediaFile, username, post.Id);
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
        public async Task<bool> LikePostAsync(string username, string postId)
        {
            var post = await this.dbContext.Posts.SingleOrDefaultAsync(x => x.Id == postId);
            var user = await this.identityService.GetUserByName(username).SingleOrDefaultAsync();

            var isPostLikedAlready = await this.dbContext.PostLikes
                .SingleOrDefaultAsync(x => x.Post == post && x.User == user) != null;

            if (post != null && user != null && !isPostLikedAlready)
            {
                this.dbContext.PostLikes.Add(new PostLike()
                {
                    Post = post,
                    User = user
                });
                await this.dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> LikeCommentAsync(string username, string commentId)
        {
            var comment = await this.dbContext.Comments
                .SingleOrDefaultAsync(x => x.Id == commentId);
            var user = await this.identityService.GetUserByName(username).SingleOrDefaultAsync();

            var isCommentLikedAlready = await this.dbContext.CommentLikes
                .SingleOrDefaultAsync(x => x.Comment == comment && x.User == user) != null;

            if (comment != null && user != null && !isCommentLikedAlready)
            {
                this.dbContext.CommentLikes.Add(new CommentLike()
                {
                    Comment = comment,
                    User = user
                });
                await this.dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> CommentPostAsync(string username, string postId, string content)
        {
            var post = await this.dbContext.Posts.SingleOrDefaultAsync(x => x.Id == postId);
            var user = await this.identityService.GetUserByName(username).SingleOrDefaultAsync();

            if (post != null && user != null)
            {
                this.dbContext.Comments.Add(new Comment()
                {
                    User = user,
                    Post = post,
                    Content = content
                });
                await this.dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<string> GetPostOwnerUsernameAsync(string postId)
        {
            return (await this.dbContext.Posts
                .Include(x => x.User)
                .SingleOrDefaultAsync(x => x.Id == postId)).User.UserName;
        }
        public async Task<string> GetCommentOwnerUsernameAsync(string commentId)
        {
            return (await this.dbContext.Comments
                .Include(x => x.User)
                .SingleOrDefaultAsync(x => x.Id == commentId)).User.UserName;
        }
        public async Task<bool> IsUserCanPostToWallAsync(string username, string WallId)
        {
            var user = await identityService.GetUserByName(username)
                .SingleOrDefaultAsync();

            var wall = await wallService.GetWallWithoutIncludables(WallId);
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
                    .Include(x => x.Members)
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
        public async Task<PostViewModel> GetShareViewAsync(string postId)
        {
            var post = await this.dbContext.Posts
                .Include(x => x.User)
                .SingleOrDefaultAsync(x => x.Id == postId);
            var viewModel = automapper.Map<PostViewModel>(post);

            viewModel.Content = new string(viewModel.Content.Take(20).ToArray());
            viewModel.IgnoreComments = true;

            return viewModel;
        }
        public async Task<bool> SharePostAsync(string username, ShareInputModel model)
        {
            var user = await identityService.GetUserByName(username)
                .SingleOrDefaultAsync();
            Wall wall = null;

            if (model.ShareLocationType == ShareLocation.FriendWall &&
                await relationsService.AreFriendsWithAsync(username, model.ShareLocation))
            {
                wall = await wallService.GetUserWallAsync(model.ShareLocation);
            }
            else if (model.ShareLocationType == ShareLocation.GroupWall &&
              await groupService.IsUserConfirmedMemberAsync(model.ShareLocation, username))
            {
                wall = await wallService.GetGroupWallAsync(model.ShareLocation);
            }
            else if (model.ShareLocationType == ShareLocation.MyWall)
            {
                wall = await wallService.GetUserWallAsync(username);
            }

            if (user != null && wall != null &&
                await IsUserCanSeePostAsync(username, model.PostId) &&
                await IsUserCanPostToWallAsync(username, wall.Id))
            {
                var sharedPost = new Post()
                {
                    Content = model.PostModel.Content,
                    Tags = GetPostTags(model.PostModel.Tags),
                    SharedPostId = model.PostId,
                    Location = wall,
                    User = user
                };
                var share = new Share()
                {
                    PostId = model.PostId,
                    User = user
                };

                if (model.ShareLocationType == ShareLocation.MyWall)
                {
                    sharedPost.VisibilityType = model.PostModel.VisibilityType;
                }
                else
                {
                    sharedPost.VisibilityType = PostVisibilityType.Public;
                }

                await this.dbContext.Posts.AddAsync(sharedPost);
                await this.dbContext.Shares.AddAsync(share);
                await this.dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> DeletePostAsync(string username, string postId)
        {
            var post = await this.dbContext.Posts
                .Include(x => x.User)
                .Include(x => x.Likes)
                .Include(x => x.Comments)
                    .ThenInclude(x => x.Likes)
                .Include(x => x.Shares)
                .SingleOrDefaultAsync(x => x.Id == postId);

            if (post != null && post.User.UserName == username)
            {
                foreach (var like in post.Likes)
                {
                    this.dbContext.PostLikes.Remove(like);
                }
                foreach (var comment in post.Comments)
                {
                    this.dbContext.Comments.Remove(comment);
                    foreach (var commentLike in comment.Likes)
                    {
                        this.dbContext.CommentLikes.Remove(commentLike);
                    }
                }
                foreach (var share in post.Shares)
                {
                    this.dbContext.Shares.Remove(share);
                }
                this.dbContext.Posts.Remove(post);
                await this.dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<PostViewModel> GetPostViewAsync(string postId)
        {
            var post = await this.dbContext.Posts
                .Include(this.dbContext.GetIncludePaths(typeof(Post)))
                .SingleOrDefaultAsync(x => x.Id == postId);

            if (post != null)
            {
                post.Comments = post.Comments.OrderBy(x => x.CreatedOn).ToArray();
                return automapper.Map<PostViewModel>(post);
            }
            return null;
        }
        private IEnumerable<string> GetPostTags(string tagsString)
        {
            IEnumerable<string> tags = new string[0];
            if (tagsString != null && tagsString.Length > 0)
            {
                tags = tagsString.Split(',').Select(x => x.Trim());
            }
            return tags;
        }
    }
}
