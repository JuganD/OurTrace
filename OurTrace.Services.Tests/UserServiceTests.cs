namespace OurTrace.Services.Tests
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using OurTrace.Data;
    using OurTrace.Data.Identity.Models;
    using OurTrace.Data.Models;
    using OurTrace.Services.Tests.StaticResources;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;
    public class UserServiceTests
    {
        private readonly IMapper automapper;
        private readonly UserService userService;
        private readonly OurTraceDbContext dbContext;

        public UserServiceTests()
        {
            var options = new DbContextOptionsBuilder<OurTraceDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;
            this.dbContext = new OurTraceDbContext(options);

            this.automapper = MapperInitializer.CreateMapper();

            var relationsService = new RelationsService(dbContext, automapper);
            this.userService = new UserService(dbContext, automapper, relationsService);
        }

        [Fact]
        public async Task PrepareUserProfileForViewAsync_ShouldReturn_ValidView()
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
                Content = "post",
                User = user,
                Location = user.Wall
            };
            var comment = new Comment()
            {
                Content = "Gosho komentira",
                User = user,
                Post = post
            };

            post.Comments.Add(comment);
            user.Posts.Add(post);

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.SaveChangesAsync();

            var actual = await this.userService.PrepareUserProfileForViewAsync(user.UserName, user.UserName);
            Assert.True(actual.IsHimself);
            Assert.Equal(0, actual.FriendsCount);
            Assert.Equal(user.UserName, actual.Username);
            Assert.Equal(1, actual.Posts.Count);
            Assert.Equal(1, actual.Posts.First().Comments.Count);
        }
        [Fact]
        public async Task GetLastNPicturesAsync_ShouldReturn_ValidView()
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
                Content = "post",
                User = user,
                Location = user.Wall,
                MediaUrl = "expectedExternalMediaUrl"
            };
            var post2 = new Post()
            {
                Content = "post2",
                User = user,
                Location = user.Wall,
                MediaUrl = "invalidInternalMediaUrl",
                IsImageOnFileSystem = true
            };

            user.Posts.Add(post);
            user.Posts.Add(post2);

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.SaveChangesAsync();

            var actual = await this.userService.GetLastNPicturesAsync(user.UserName, 5);
            Assert.Equal(1, actual.InternalIds.Count);
            Assert.Equal(1, actual.ExternalUrls.Count);

        }
        [Fact]
        public async Task UserExistsAsync_ShouldReturn_ValidResult()
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

            var actual = await this.userService.UserExistsAsync(user.UserName);
            Assert.True(actual);
        }
    }
}
