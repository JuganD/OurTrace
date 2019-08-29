namespace OurTrace.Services.Tests
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using OurTrace.App.Models.InputModels.Posts;
    using OurTrace.App.Models.InputModels.Share;
    using OurTrace.Data;
    using OurTrace.Data.Identity.Models;
    using OurTrace.Data.Models;
    using OurTrace.Services.Tests.StaticResources;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;
    public class PostServiceTests
    {
        private readonly IMapper automapper;
        private readonly PostService postService;
        private readonly OurTraceDbContext dbContext;

        public PostServiceTests()
        {
            var options = new DbContextOptionsBuilder<OurTraceDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;
            this.dbContext = new OurTraceDbContext(options);

            this.automapper = MapperInitializer.CreateMapper();
            var groupService = new GroupService(dbContext, automapper);
            var relationsService = new RelationsService(dbContext, automapper);
            FileService fileService = null; // mocked - not used
            this.postService = new PostService(dbContext, relationsService, groupService, fileService, automapper);
        }

        [Fact]
        public async Task CreateNewPostAsync_ShouldAdd_SingleNewPost()
        {
            var user = new OurTraceUser()
            {
                UserName = "Gosho",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "GOSHO",
                PasswordHash = "00000",
                Wall = new Wall()
            };
            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.SaveChangesAsync();

            var postInputModel = new CreatePostInputModel()
            {
                Content = "Post content",
                IsGroupPost = false,
                Location = user.Wall.Id,
                VisibilityType = PostVisibilityType.FriendsOnly
            };

            var actual = await this.postService.CreateNewPostAsync(user.UserName, postInputModel, false);
            Assert.True(actual);

            var actualPostCount = await this.dbContext.Posts.CountAsync();
            var actualPost = await this.dbContext.Posts
                .Include(x => x.User)
                .SingleOrDefaultAsync();

            Assert.Equal(1, actualPostCount);
            Assert.NotNull(actualPost);
            Assert.Equal(user.UserName, actualPost.User.UserName);
            Assert.Equal(postInputModel.Content, actualPost.Content);
            Assert.Equal(postInputModel.VisibilityType, actualPost.VisibilityType);
            Assert.Equal(postInputModel.Location, actualPost.LocationId);
        }
        [Fact]
        public async Task LikePostAsync_ShouldAffect_SinglePost()
        {
            var user = new OurTraceUser()
            {
                UserName = "Gosho",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "GOSHO",
                PasswordHash = "00000",
                Wall = new Wall()
            };

            var post = new Post()
            {
                Content = "Post content",
                Location = user.Wall,
                User = user,
                VisibilityType = PostVisibilityType.FriendsOnly
            };

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Posts.AddAsync(post);
            await this.dbContext.SaveChangesAsync();


            var actual = await this.postService.LikePostAsync(user.UserName, post.Id);
            Assert.True(actual);
            Assert.Equal(1, post.Likes.Count);
        }
        [Fact]
        public async Task LikeCommentAsync_ShouldAffect_SingleComment()
        {
            var user = new OurTraceUser()
            {
                UserName = "Gosho",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "GOSHO",
                PasswordHash = "00000",
                Wall = new Wall()
            };

            var post = new Post()
            {
                Content = "Post content",
                Location = user.Wall,
                User = user,
                VisibilityType = PostVisibilityType.FriendsOnly
            };
            var comment = new Comment()
            {
                Content = "zadara",
                Post = post,
                User = user,
            };
            post.Comments.Add(comment);

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Posts.AddAsync(post);
            await this.dbContext.SaveChangesAsync();


            var actual = await this.postService.LikeCommentAsync(user.UserName, comment.Id);
            Assert.True(actual);
            Assert.Equal(1, comment.Likes.Count);
        }
        [Fact]
        public async Task CommentPostAsync_ShouldCreate_SingleComment()
        {
            var user = new OurTraceUser()
            {
                UserName = "Gosho",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "GOSHO",
                PasswordHash = "00000",
                Wall = new Wall()
            };

            var post = new Post()
            {
                Content = "Post content",
                Location = user.Wall,
                User = user,
                VisibilityType = PostVisibilityType.FriendsOnly
            };

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Posts.AddAsync(post);
            await this.dbContext.SaveChangesAsync();

            var comment = new Comment()
            {
                Content = "zadara",
                Post = post,
                User = user,
            };

            var expectedString = "zadara";
            var actualBool = await this.postService.CommentPostAsync(user.UserName, post.Id, expectedString);
            Assert.True(actualBool);

            var actualString = post.Comments.FirstOrDefault().Content;
            Assert.Equal(expectedString, actualString);
        }
        [Fact]
        public async Task GetPostOwnerUsernameAsync_ShouldReturn_CorrectValue()
        {
            var user = new OurTraceUser()
            {
                UserName = "Gosho",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "GOSHO",
                PasswordHash = "00000",
                Wall = new Wall()
            };

            var post = new Post()
            {
                Content = "Post content",
                Location = user.Wall,
                User = user,
                VisibilityType = PostVisibilityType.FriendsOnly
            };

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Posts.AddAsync(post);
            await this.dbContext.SaveChangesAsync();

            var actual = await this.postService.GetPostOwnerUsernameAsync( post.Id);
            Assert.Equal(user.UserName, actual);
        }
        [Fact]
        public async Task GetCommentOwnerUsernameAsync_ShouldReturn_CorrectValue()
        {
            var user = new OurTraceUser()
            {
                UserName = "Gosho",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "GOSHO",
                PasswordHash = "00000",
                Wall = new Wall()
            };

            var post = new Post()
            {
                Content = "Post content",
                Location = user.Wall,
                User = user,
                VisibilityType = PostVisibilityType.FriendsOnly
            };
            var comment = new Comment()
            {
                Content = "asd",
                Post = post,
                User = user
            };
            post.Comments.Add(comment);

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Posts.AddAsync(post);
            await this.dbContext.SaveChangesAsync();

            var actual = await this.postService.GetCommentOwnerUsernameAsync(comment.Id);
            Assert.Equal(user.UserName, actual);
        }
        [Fact]
        public async Task IsUserCanPostToWallAsync_ShouldReturn_CorrectValues()
        {
            var user = new OurTraceUser()
            {
                UserName = "Gosho",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "GOSHO",
                PasswordHash = "00000",
                Wall = new Wall()
            };
            var user2 = new OurTraceUser()
            {
                UserName = "Pesho",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "PESHO",
                PasswordHash = "00000",
                Wall = new Wall()
            };
            var user3 = new OurTraceUser()
            {
                UserName = "Mitko",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "MITKO",
                PasswordHash = "00000",
                Wall = new Wall()
            };

            var friendShipBetweenUser2AndUser1 = new Friendship()
            {
                Sender = user2,
                Recipient = user,
                AcceptedOn = DateTime.Now
            };

            var post = new Post()
            {
                Content = "Post content",
                Location = user.Wall,
                User = user,
                VisibilityType = PostVisibilityType.FriendsOnly
            };


            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Users.AddAsync(user2);
            await this.dbContext.Users.AddAsync(user3);
            await this.dbContext.Posts.AddAsync(post);
            await this.dbContext.Friendships.AddAsync(friendShipBetweenUser2AndUser1);
            await this.dbContext.SaveChangesAsync();

            var actual1 = await this.postService.IsUserCanPostToWallAsync(user3.UserName,user.Wall.Id);
            Assert.False(actual1);

            var actual2 = await this.postService.IsUserCanPostToWallAsync(user2.UserName, user.Wall.Id);
            Assert.True(actual2);

            var actual3 = await this.postService.IsUserCanPostToWallAsync(user.UserName, user.Wall.Id);
            Assert.True(actual3);
        }
        [Fact]
        public async Task IsUserCanSeePostAsync_ShouldReturn_CorrectValue()
        {
            var user = new OurTraceUser()
            {
                UserName = "Gosho",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "GOSHO",
                PasswordHash = "00000",
                Wall = new Wall()
            };
            var user2 = new OurTraceUser()
            {
                UserName = "Pesho",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "PESHO",
                PasswordHash = "00000",
                Wall = new Wall()
            };
            var user3 = new OurTraceUser()
            {
                UserName = "Mitko",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "MITKO",
                PasswordHash = "00000",
                Wall = new Wall()
            };

            var friendShipBetweenUser2AndUser1 = new Friendship()
            {
                Sender = user2,
                Recipient = user,
                AcceptedOn = DateTime.Now
            };

            var post = new Post()
            {
                Content = "Post content",
                Location = user.Wall,
                User = user,
                VisibilityType = PostVisibilityType.FriendsOnly
            };


            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Users.AddAsync(user2);
            await this.dbContext.Users.AddAsync(user3);
            await this.dbContext.Posts.AddAsync(post);
            await this.dbContext.Friendships.AddAsync(friendShipBetweenUser2AndUser1);
            await this.dbContext.SaveChangesAsync();

            var actual1 = await this.postService.IsUserCanSeePostAsync(user3.UserName, post.Id);
            Assert.False(actual1);

            var actual2 = await this.postService.IsUserCanSeePostAsync(user2.UserName, post.Id);
            Assert.True(actual2);

            var actual3 = await this.postService.IsUserCanSeePostAsync(user.UserName, post.Id);
            Assert.True(actual3);
        }
        [Fact]
        public async Task SharePostAsync_ShouldShare_Correctly()
        {
            var user = new OurTraceUser()
            {
                UserName = "Gosho",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "GOSHO",
                PasswordHash = "00000",
                Wall = new Wall()
            };
            var user2 = new OurTraceUser()
            {
                UserName = "Pesho",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "PESHO",
                PasswordHash = "00000",
                Wall = new Wall()
            };
            var user3 = new OurTraceUser()
            {
                UserName = "Mitko",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "MITKO",
                PasswordHash = "00000",
                Wall = new Wall()
            };

            var friendShipBetweenUser2AndUser1 = new Friendship()
            {
                Sender = user2,
                Recipient = user,
                AcceptedOn = DateTime.Now
            };

            var post = new Post()
            {
                Content = "Post content",
                Location = user.Wall,
                User = user,
                VisibilityType = PostVisibilityType.FriendsOnly
            };


            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Users.AddAsync(user2);
            await this.dbContext.Users.AddAsync(user3);
            await this.dbContext.Posts.AddAsync(post);
            await this.dbContext.Friendships.AddAsync(friendShipBetweenUser2AndUser1);
            await this.dbContext.SaveChangesAsync();

            var shareInputModel = new ShareInputModel()
            {
                PostId = post.Id,
                PostModel = new CreatePostInputModel()
                {
                    Content = "Shared Content",
                    Location = user2.WallId,
                    VisibilityType = PostVisibilityType.Public
                },
                ShareLocation = user.UserName,
                ShareLocationType = ShareLocation.FriendWall
            };
            var actual1 = await this.postService.SharePostAsync(user2.UserName, shareInputModel);
            Assert.True(actual1);

            var actual3 = await this.postService.SharePostAsync(user3.UserName, shareInputModel);
            Assert.False(actual3);

            shareInputModel.ShareLocation = user.UserName;
            shareInputModel.ShareLocationType = ShareLocation.MyWall;
            var actual2 = await this.postService.SharePostAsync(user.UserName, shareInputModel);
            Assert.True(actual2);
        }
        [Fact]
        public async Task DeletePostAsync_ShouldDelete_SinglePost()
        {
            var user = new OurTraceUser()
            {
                UserName = "Gosho",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "GOSHO",
                PasswordHash = "00000",
                Wall = new Wall()
            };

            var post1 = new Post()
            {
                Content = "Post content",
                Location = user.Wall,
                User = user,
                VisibilityType = PostVisibilityType.FriendsOnly
            };
            var post2 = new Post()
            {
                Content = "Second Post content",
                Location = user.Wall,
                User = user,
                VisibilityType = PostVisibilityType.FriendsOnly
            };

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Posts.AddAsync(post1);
            await this.dbContext.Posts.AddAsync(post2);
            await this.dbContext.SaveChangesAsync();

            var actual = await this.postService.DeletePostAsync(user.UserName, post1.Id);
            Assert.True(actual);

            var postsCount = await this.dbContext.Posts.CountAsync();
            Assert.Equal(1, postsCount);

            var actual2 = await this.postService.DeletePostAsync(user.UserName, post2.Id);
            Assert.True(actual2);
            postsCount = await this.dbContext.Posts.CountAsync();
            Assert.Equal(0, postsCount);
        }
        
    }
}
