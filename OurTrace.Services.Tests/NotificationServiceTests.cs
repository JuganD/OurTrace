namespace OurTrace.Services.Tests
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using OurTrace.Data;
    using OurTrace.Data.Identity.Models;
    using OurTrace.Data.Models;
    using OurTrace.Services.Models;
    using OurTrace.Services.Tests.StaticResources;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;
    public class NotificationServiceTests
    {
        private readonly IMapper automapper;
        private readonly NotificationService notificationService;
        private readonly OurTraceDbContext dbContext;

        public NotificationServiceTests()
        {
            var options = new DbContextOptionsBuilder<OurTraceDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;
            this.dbContext = new OurTraceDbContext(options);

            this.automapper = MapperInitializer.CreateMapper();
            this.notificationService = new NotificationService(dbContext, automapper);
        }
        [Fact]
        public async Task AddNotificationToUserAsync_ShouldAdd_Successfully()
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

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.SaveChangesAsync();

            var notificationModel = new NotificationServiceModel()
            {
                Content = "NotificContent",
                Controller = "Group",
                Action = "Open",
                Username = user.UserName,
                ElementId = "1"
            };

            await this.notificationService.AddNotificationToUserAsync(notificationModel);

            var expectedNotification = new Notification()
            {
                Content = "NotificContent",
                Controller = "Group",
                Action = "Open",
                User = user,
                ElementId = "1",
                Seen = false
            };
            var actualNotificatiionsCount = await this.dbContext.Notifications.CountAsync();
            var actualNotificatiion = await this.dbContext.Notifications.FirstOrDefaultAsync();

            Assert.Equal(1, actualNotificatiionsCount);
            AssertExpectedAndActualNotificationProperties(expectedNotification, actualNotificatiion);
        }
        [Fact]
        public async Task GetUserNotificationsAsync_ShouldReturn_AllNotifications()
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

            var notification1 = new Notification()
            {
                Content = "NotificContent",
                Controller = "Group",
                Action = "Open",
                User = user,
                ElementId = "1",
                Seen = true
            };
            var notification2 = new Notification()
            {
                Content = "NotificContent2",
                Controller = "Asd",
                Action = "Dsa",
                User = user,
                ElementId = "181904549804dfasd1d9a0s4",
                Seen = false
            };

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Notifications.AddAsync(notification1);
            await this.dbContext.Notifications.AddAsync(notification2);
            await this.dbContext.SaveChangesAsync();


            var actual1 = await this.notificationService.GetUserNotificationsAsync(user.UserName, 1, false);
            Assert.Equal(1, actual1.Count);
            Assert.Equal(notification1.Content, actual1.FirstOrDefault().Content);

            var actual2 = await this.notificationService.GetUserNotificationsAsync(user.UserName, 2, false);
            Assert.Equal(2, actual2.Count);
            Assert.Equal(notification1.Content, actual2.FirstOrDefault().Content);
            Assert.Equal(notification2.Content, actual2.LastOrDefault().Content);

            var actual3 = await this.notificationService.GetUserNotificationsAsync(user.UserName, 2, true);
            Assert.Equal(2, actual3.Count);
            Assert.Equal(notification2.Content, actual3.FirstOrDefault().Content);
            Assert.Equal(notification1.Content, actual3.LastOrDefault().Content);

        }
        [Fact]
        public async Task MarkAllNotificationsAsSeenAsync_ShouldModify_AllNotifications()
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

            var notification1 = new Notification()
            {
                Content = "NotificContent",
                Controller = "Group",
                Action = "Open",
                User = user,
                ElementId = "1",
                Seen = true
            };
            var notification2 = new Notification()
            {
                Content = "NotificContent2",
                Controller = "Asd",
                Action = "Dsa",
                User = user,
                ElementId = "181904549804dfasd1d9a0s4",
                Seen = false
            };

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Notifications.AddAsync(notification1);
            await this.dbContext.Notifications.AddAsync(notification2);
            await this.dbContext.SaveChangesAsync();


            await this.notificationService.MarkAllNotificationsAsSeenAsync(user.UserName);
            var actual = (await this.dbContext.Notifications.ToListAsync())
                .Select(x => x.Seen)
                .ToList();

            Assert.All(actual, x => x = true);
        }
        [Fact]
        public async Task MarkNotificationAsSeenAndReturnItAsync_ShouldModify_SingleNotification()
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

            var notification1 = new Notification()
            {
                Content = "NotificContent",
                Controller = "Group",
                Action = "Open",
                User = user,
                ElementId = "1",
                Seen = true
            };
            var notification2 = new Notification()
            {
                Id = "123",
                Content = "NotificContent2",
                Controller = "Asd",
                Action = "Dsa",
                User = user,
                ElementId = "181904549804dfasd1d9a0s4",
                Seen = false
            };

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Notifications.AddAsync(notification1);
            await this.dbContext.Notifications.AddAsync(notification2);
            await this.dbContext.SaveChangesAsync();


            var actual = await this.notificationService.MarkNotificationAsSeenAndReturnItAsync(notification2.Id);
            var databaseActualCount = await this.dbContext.Notifications.CountAsync(x=>x.Seen==true);

            Assert.Equal(2, databaseActualCount);
            var actualNotification = new Notification()
            {
                Content = actual.Content,
                Controller = actual.Controller,
                Action = actual.Action,
                ElementId = actual.ElementId,
                Seen = actual.Seen
            };
            AssertExpectedAndActualNotificationProperties(actualNotification, notification2);

        }

        private void AssertExpectedAndActualNotificationProperties(Notification firstNotification, Notification secondNotification)
        {
            foreach (var property in firstNotification.GetType().GetProperties().Where(x => x.GetValue(firstNotification) != null))
            {
                // Ids are supposed to be different
                if (property.Name == nameof(firstNotification.Id)) continue;
                // Dates are always different
                if (property.Name == nameof(firstNotification.DateIssued)) continue;

                var expectedValue = property.GetValue(firstNotification);
                var actualValue = property.GetValue(secondNotification);
                Assert.Equal(expectedValue, actualValue);
            }
        }
    }
}
