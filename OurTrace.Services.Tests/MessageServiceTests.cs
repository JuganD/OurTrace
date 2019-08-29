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
    public class MessageServiceTests
    {
        private readonly IMapper automapper;
        private readonly MessageService messageService;
        private readonly OurTraceDbContext dbContext;

        public MessageServiceTests()
        {
            var options = new DbContextOptionsBuilder<OurTraceDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;
            this.dbContext = new OurTraceDbContext(options);

            this.automapper = MapperInitializer.CreateMapper();
            var relationsService = new RelationsService(dbContext, automapper);
            this.messageService = new MessageService(dbContext, automapper, relationsService);
        }
        [Fact]
        public async Task GetAllUsernamesOfMessageIssuersAsync_ShouldReturn_ValidCollection()
        {
            var user = new OurTraceUser()
            {
                UserName = "Gosho",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "GOSHO",
                PasswordHash = "00000"
            };
            var user2 = new OurTraceUser()
            {
                UserName = "Pesho",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "PESHO",
                PasswordHash = "00000"
            };
            var user3 = new OurTraceUser()
            {
                UserName = "Mitko",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "MITKO",
                PasswordHash = "00000"
            };

            var friendshipBetweenUserAndUser2 = new Friendship()
            {
                Sender = user,
                Recipient = user2
            };
            var messageBetweenUser3AndUser = new Message()
            {
                Sender = user3,
                Recipient = user,
                Content = "Zdr gosho"
            };
            var messageBetweenUser2AndUser = new Message()
            {
                Sender = user2,
                Recipient = user,
                Content = "Zdr gosho, az sam"
            };

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Users.AddAsync(user2);
            await this.dbContext.Users.AddAsync(user3);
            await this.dbContext.Friendships.AddAsync(friendshipBetweenUserAndUser2);
            await this.dbContext.Messages.AddAsync(messageBetweenUser2AndUser);
            await this.dbContext.Messages.AddAsync(messageBetweenUser3AndUser);
            await this.dbContext.SaveChangesAsync();

            // Should return user2 and user3 usernames, because
            // it should get all friends of user1 and all messagers of user1
            var actual = await this.messageService.GetAllUsernamesOfMessageIssuersAsync(user.UserName);
            Assert.Contains(actual, x => x == user2.UserName);
            Assert.Contains(actual, x => x == user3.UserName);
        }
        [Fact]
        public async Task GetMessagesAsync_ShouldReturn_AllMessages()
        {
            var user = new OurTraceUser()
            {
                UserName = "Gosho",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "GOSHO",
                PasswordHash = "00000"
            };
            var user2 = new OurTraceUser()
            {
                UserName = "Pesho",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "PESHO",
                PasswordHash = "00000"
            };

            var message1 = new Message()
            {
                Sender = user,
                Recipient = user2,
                Content = "Zdr pesho"
            };
            var message2 = new Message()
            {
                Sender = user2,
                Recipient = user,
                Content = "Zdr gosho, az sam"
            };

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Users.AddAsync(user2);
            await this.dbContext.Messages.AddAsync(message1);
            await this.dbContext.Messages.AddAsync(message2);
            await this.dbContext.SaveChangesAsync();


            var actual = await this.messageService.GetMessagesAsync(user.UserName,user2.UserName);
            Assert.Equal(2, actual.Count);
            Assert.Contains(actual, x => x.Content == message1.Content);
            Assert.Contains(actual, x => x.Content == message2.Content);
            Assert.NotEqual(actual.First(), actual.Last());
        }
        [Fact]
        public async Task SendMessageAsync_ShouldSend_CorrectValues()
        {
            var user = new OurTraceUser()
            {
                UserName = "Gosho",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "GOSHO",
                PasswordHash = "00000"
            };
            var user2 = new OurTraceUser()
            {
                UserName = "Pesho",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "PESHO",
                PasswordHash = "00000"
            };

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Users.AddAsync(user2);
            await this.dbContext.SaveChangesAsync();

            var messageExpected = new Message()
            {
                Sender = user,
                Recipient = user2,
                Content = "Zdr pesho"
            };

            await this.messageService.SendMessageAsync(user.UserName,user2.UserName,messageExpected.Content);

            var actualCount = await this.dbContext.Messages.CountAsync();
            var actual = await this.dbContext.Messages
                .Include(x=>x.Sender)
                .Include(x=>x.Recipient)
                .FirstOrDefaultAsync();

            Assert.Equal(1, actualCount);
            Assert.NotNull(actual);
            Assert.Equal(messageExpected.Content, actual.Content);
            Assert.Equal(messageExpected.Sender.UserName, actual.Sender.UserName);
            Assert.Equal(messageExpected.Recipient.UserName, actual.Recipient.UserName);
        }

    }
}
