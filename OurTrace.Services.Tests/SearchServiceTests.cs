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
    public class SearchServiceTests
    {
        private readonly IMapper automapper;
        private readonly SearchService searchService;
        private readonly OurTraceDbContext dbContext;

        public SearchServiceTests()
        {
            var options = new DbContextOptionsBuilder<OurTraceDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;
            this.dbContext = new OurTraceDbContext(options);

            this.automapper = MapperInitializer.CreateMapper();
            FileService fileService = null;
            var relationsService = new RelationsService(dbContext, automapper);
            var groupService = new GroupService(dbContext, automapper);
            var postService = new PostService(dbContext, relationsService, groupService, fileService, automapper);
            this.searchService = new SearchService(dbContext, automapper, postService);
        }

        [Fact]
        public async Task SearchForCommentsAsync_ShouldReturn_CorrectValues()
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

            var actual = await this.searchService.SearchForCommentsAsync("komentira", "Gosho");
            Assert.Equal(1, actual.Values.Count);
            Assert.Equal(user.UserName, actual.Values.First().Content);
            Assert.Equal(comment.Content, actual.Values.First().DescriptiveContent);
        }
        [Fact]
        public async Task SearchForGroupsAsync_ShouldReturn_CorrectValues()
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
            var group = new Group()
            {
                Name = "GrupataNaGosho"
            };

            group.Members.Add(new UserGroup()
            {
                User = user,
                Group = group,
                ConfirmedMember = true
            });

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Groups.AddAsync(group);
            await this.dbContext.SaveChangesAsync();

            var actual = await this.searchService.SearchForGroupsAsync("Grupata");
            Assert.Equal(1, actual.Values.Count);
            Assert.Equal(group.Name, actual.Values.First().Content);
        }
        [Fact]
        public async Task SearchForUsersAsync_ShouldReturn_CorrectValues()
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

            var actual = await this.searchService.SearchForUsersAsync("gos");
            Assert.Equal(1, actual.Values.Count);
            Assert.Equal(user.UserName, actual.Values.First().Content);
        }
        [Fact]
        public async Task SearchForPostsAsync_ShouldReturn_CorrectValues()
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
                Content = "Qko bace",
                Tags = new List<string>() {
                    "mnogo","qki","tagove"
                },
                User = user,
                Location = user.Wall
            };
            user.Posts.Add(post);

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.SaveChangesAsync();

            var actual1 = await this.searchService.SearchForPostsAsync("bace",user.UserName);
            Assert.Equal(1, actual1.Values.Count);
            Assert.Equal(user.UserName, actual1.Values.First().Content);

            var actual2 = await this.searchService.SearchForPostsAsync("tagove", user.UserName);
            Assert.Equal(1, actual2.Values.Count);
        }
        [Fact]
        public async Task SearchForEverythingAsync_ShouldReturn_CorrectValues()
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
                Content = "Qko bace",
                Tags = new List<string>() {
                    "mnogo","qki","tagove","gosho"
                },
                User = user,
                Location = user.Wall
            };
            var group = new Group()
            {
                Name = "Goshovata grupa",
                Creator = user,
            };
            var comment = new Comment()
            {
                Content = "Goshov komentar",
                User = user,
                Post = post
            };
            post.Comments.Add(comment);
            user.Posts.Add(post);

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Groups.AddAsync(group);
            await this.dbContext.SaveChangesAsync();

            var actual = await this.searchService.SearchForEverythingAsync("gosho", user.UserName);
            Assert.Equal(1, actual.Comments.Count);
            Assert.Equal(1, actual.Groups.Count);
            Assert.Equal(1, actual.Users.Count);
            Assert.Equal(1, actual.Posts.Count);
        }
    }
}
