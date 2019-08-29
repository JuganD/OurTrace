namespace OurTrace.Services.Tests
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using OurTrace.Data;
    using OurTrace.Data.Identity.Models;
    using OurTrace.Data.Models;
    using OurTrace.Services.Tests.StaticResources;
    using System;
    using System.Threading.Tasks;
    using Xunit;
    public class HomeServiceTests
    {
        private readonly IMapper automapper;
        private readonly HomeService homeService;
        private readonly OurTraceDbContext dbContext;

        public HomeServiceTests()
        {
            var options = new DbContextOptionsBuilder<OurTraceDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;
            this.dbContext = new OurTraceDbContext(options);

            this.automapper = MapperInitializer.CreateMapper();
            var advertService = new AdvertService(dbContext, automapper);
            var relationsService = new RelationsService(dbContext, automapper);
            var groupService = new GroupService(dbContext, automapper);
            this.homeService = new HomeService(dbContext, automapper, advertService, relationsService, groupService);
        }
        [Fact]
        public async Task GetNewsfeedViewModelAsync_ShouldReturn_ValidModel()
        {
            var user = new OurTraceUser()
            {
                Id = "123",
                UserName = "Gosho",
                FullName = "Gosho Goshev",
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
                Id = "1234",
                UserName = "Pesho",
                FullName = "Gosho Goshev",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "PESHO",
                PasswordHash = "00000",
                Wall = new Wall()
            };

            var follow = new Follow()
            {
                Sender = user2,
                Recipient = user
            };
            var following = new Follow()
            {
                Sender = user,
                Recipient = user2
            };

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Users.AddAsync(user2);
            await this.dbContext.Follows.AddAsync(follow);
            await this.dbContext.Follows.AddAsync(following);
            await this.dbContext.SaveChangesAsync();

            var actual = await this.homeService.GetNewsfeedViewModelAsync(user.Id);
            Assert.Equal(user.FullName, actual.FullName);
            Assert.Equal(1, actual.Followers);
            Assert.Equal(1, actual.Following);
        }
        [Fact]
        public async Task GetPostsForNewsfeedAsync_ShouldReturn_ValidPosts()
        {
            var user = new OurTraceUser()
            {
                Id = "123",
                UserName = "Gosho",
                FullName = "Gosho Goshev",
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
                Id = "1234",
                UserName = "Pesho",
                FullName = "Gosho Goshev",
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
                Id = "12345",
                UserName = "Qvor",
                FullName = "Gosho Goshev",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "QVOR",
                PasswordHash = "00000",
                Wall = new Wall()
            };

            var user2Post1 = new Post()
            {
                Content = "PostContent",
                User = user2,
                Location = user2.Wall,
                VisibilityType = PostVisibilityType.Public
            };
            var user2Post2 = new Post()
            {
                Content = "FriendsOnlyPostContent",
                User = user2,
                Location = user2.Wall,
                VisibilityType = PostVisibilityType.FriendsOnly
            };
            var user3Post = new Post()
            {
                Content = "FriendshipPost",
                User = user3,
                Location = user3.Wall,
                VisibilityType = PostVisibilityType.FriendsOnly
            };

            user2.Posts.Add(user2Post1);
            user2.Posts.Add(user2Post2);
            user3.Posts.Add(user3Post);

            var follow = new Follow()
            {
                Sender = user2,
                Recipient = user
            };
            var following = new Follow()
            {
                Sender = user,
                Recipient = user2
            };
            var user1AndUser3Friendship = new Friendship()
            {
                Sender = user,
                Recipient = user3,
                AcceptedOn = DateTime.Now
            };

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Users.AddAsync(user2);
            await this.dbContext.Users.AddAsync(user3);
            await this.dbContext.Follows.AddAsync(follow);
            await this.dbContext.Follows.AddAsync(following);
            await this.dbContext.Friendships.AddAsync(user1AndUser3Friendship);
            await this.dbContext.SaveChangesAsync();

            var actual = await this.homeService.GetPostsForNewsfeedAsync(user.Id, false);
            // 2 posts expected - one from follow and one from friendship
            // Third post is friendsOnly, but user1 and user2 are only 
            // following each other, so we should not be seeing it.
            Assert.Equal(2, actual.Count);

            Assert.Contains(actual, x => x.Content == user2Post1.Content);
            Assert.Contains(actual, x => x.Content == user3Post.Content);
        }

        [Fact]
        public async Task GetUserIdFromName_ShouldReturn_ValidId()
        {
            var user = new OurTraceUser()
            {
                Id = "ShouldReturnThis",
                UserName = "Gosho",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "GOSHO",
                PasswordHash = "00000"
            };

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.SaveChangesAsync();

            var actual = await this.homeService.GetUserIdFromName(user.UserName);
            Assert.Equal(user.Id, actual);
        }
    }
}
