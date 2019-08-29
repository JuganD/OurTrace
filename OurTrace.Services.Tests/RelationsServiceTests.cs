namespace OurTrace.Services.Tests
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using OurTrace.Data;
    using OurTrace.Data.Identity.Models;
    using OurTrace.Data.Models;
    using OurTrace.Services.Tests.StaticResources;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;
    public class RelationsServiceTests
    {
        private readonly IMapper automapper;
        private readonly RelationsService relationsService;
        private readonly OurTraceDbContext dbContext;

        public RelationsServiceTests()
        {
            var options = new DbContextOptionsBuilder<OurTraceDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;
            this.dbContext = new OurTraceDbContext(options);

            this.automapper = MapperInitializer.CreateMapper();
            this.relationsService = new RelationsService(dbContext, automapper);
        }

        [Fact]
        public async Task AreFriendsWithAsync_ShouldReturn_CorrectValue()
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
            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Users.AddAsync(user2);
            await this.dbContext.SaveChangesAsync();

            var actual1 = await this.relationsService.AreFriendsWithAsync(user.UserName, user2.UserName);
            Assert.False(actual1);

            var friendShip = new Friendship()
            {
                Sender = user,
                Recipient = user2,
                AcceptedOn = DateTime.Now
            };
            await this.dbContext.Friendships.AddAsync(friendShip);
            await this.dbContext.SaveChangesAsync();

            var actual2 = await this.relationsService.AreFriendsWithAsync(user.UserName, user2.UserName);
            Assert.True(actual2);
        }
        [Fact]
        public async Task IsFollowingAsync_ShouldReturn_CorrectValue()
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


            var follow = new Follow()
            {
                Sender = user,
                Recipient = user2
            };

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Users.AddAsync(user2);
            await this.dbContext.Follows.AddAsync(follow);
            await this.dbContext.SaveChangesAsync();

            var actual1 = await this.relationsService.IsFollowingAsync(user.UserName, user2.UserName);
            Assert.True(actual1);

            var actual2 = await this.relationsService.IsFollowingAsync(user, user2);
            Assert.True(actual2);
        }
        [Fact]
        public async Task AddFriendshipAsync_ShouldAdd_SingleValue()
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


            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Users.AddAsync(user2);
            await this.dbContext.SaveChangesAsync();

            await this.relationsService.AddFriendshipAsync(user.UserName, user2.UserName);
            
            var actual = await this.dbContext.Friendships.SingleOrDefaultAsync();
            Assert.Equal(user, actual.Sender);
            Assert.Equal(user2, actual.Recipient);
            Assert.Null(actual.AcceptedOn);
        }
        [Fact]
        public async Task RevokeFriendshipAsync_ShouldDelete_SingleFriendship()
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

            var friendship = new Friendship()
            {
                Sender = user,
                Recipient = user2,
                AcceptedOn = DateTime.Now
            };

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Users.AddAsync(user2);
            await this.dbContext.Friendships.AddAsync(friendship);
            await this.dbContext.SaveChangesAsync();

            await this.relationsService.RevokeFriendshipAsync(user.UserName, user2.UserName);

            var actual = await this.dbContext.Friendships.CountAsync();
            Assert.Equal(0, actual);
        }
        [Fact]
        public async Task AddFollowerAsync_ShouldAdd_CorrectValue()
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

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Users.AddAsync(user2);
            await this.dbContext.SaveChangesAsync();

            await this.relationsService.AddFollowerAsync(user.UserName, user2.UserName);

            var actual = user.Following.Count;
            Assert.Equal(1, actual);
        }
        [Fact]
        public async Task RevokeFollowingAsync_ShouldDelete_CorrectValue()
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

            var follow = new Follow()
            {
                Sender = user,
                Recipient = user2
            };

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Users.AddAsync(user2);
            await this.dbContext.Follows.AddAsync(follow);
            await this.dbContext.SaveChangesAsync();

            await this.relationsService.RevokeFollowingAsync(user.UserName, user2.UserName);

            var actual = user.Following.Count;
            Assert.Equal(0, actual);
        }
        [Fact]
        public async Task GetFriendsOfFriendsAsync_ShouldReturn_AnyCorrectValue()
        {
            // Note: this is random function
            // Testing with more than one values
            // may not give expected results... And THAT is expected.

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
                UserName = "Qvor",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "QVOR",
                PasswordHash = "00000",
                Wall = new Wall()
            };

            var friendship = new Friendship()
            {
                Sender = user,
                Recipient = user2,
                AcceptedOn = DateTime.Now
            };
            var notAcceptedFriendship = new Friendship()
            {
                Sender = user,
                Recipient = user3
            };
            var friendOfFriendEXPECTED = new Friendship()
            {
                Sender = user2,
                Recipient = user3,
                AcceptedOn = DateTime.Now
            };

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Users.AddAsync(user2);
            await this.dbContext.Friendships.AddAsync(friendship);
            await this.dbContext.Friendships.AddAsync(notAcceptedFriendship);
            await this.dbContext.Friendships.AddAsync(friendOfFriendEXPECTED);
            await this.dbContext.SaveChangesAsync();

            var actualList = await this.relationsService.GetFriendsOfFriendsAsync(user.UserName, 5);
            Assert.Equal(1, actualList.Count);

            var actual = actualList.SingleOrDefault();
            Assert.Equal(friendOfFriendEXPECTED.Recipient.UserName, actual.Username);
            Assert.Equal(friendOfFriendEXPECTED.Sender.UserName, actual.HisFriendUsername);
        }
        [Fact]
        public async Task GetPendingFriendRequestsAsync_ShouldReturn_CorrectValues()
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
                UserName = "Qvor",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "QVOR",
                PasswordHash = "00000",
                Wall = new Wall()
            };

            var friendship = new Friendship()
            {
                Sender = user2,
                Recipient = user
            };
            var friendship2 = new Friendship()
            {
                Sender = user3,
                Recipient = user
            };

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Users.AddAsync(user2);
            await this.dbContext.Friendships.AddAsync(friendship);
            await this.dbContext.Friendships.AddAsync(friendship2);
            await this.dbContext.SaveChangesAsync();

            var actual = await this.relationsService.GetPendingFriendRequestsAsync(user.UserName);
            Assert.Equal(2, actual.Count);
        }
        [Fact]
        public async Task GetFriendsUsernamesAsync_ShouldReturn_CorrectValue()
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
                UserName = "Qvor",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "QVOR",
                PasswordHash = "00000",
                Wall = new Wall()
            };

            var friendship = new Friendship()
            {
                Sender = user2,
                Recipient = user,
                AcceptedOn = DateTime.Now
            };
            var notAcceptedFriendship = new Friendship()
            {
                Sender = user3,
                Recipient = user
            };

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Users.AddAsync(user2);
            await this.dbContext.Friendships.AddAsync(friendship);
            await this.dbContext.Friendships.AddAsync(notAcceptedFriendship);
            await this.dbContext.SaveChangesAsync();

            var actual = await this.relationsService.GetFriendsUsernamesAsync(user.UserName);
            Assert.Equal(1, actual.Count);
            Assert.Equal(user2.UserName, actual.First());
        }
    }
}
