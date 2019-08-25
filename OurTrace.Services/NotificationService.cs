using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OurTrace.App.Models.ViewModels.Notification;
using OurTrace.Data;
using OurTrace.Data.Models;
using OurTrace.Services.Abstraction;
using OurTrace.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurTrace.Services
{
    public class NotificationService : INotificationService
    {
        private readonly OurTraceDbContext dbContext;
        private readonly IMapper automapper;
        private readonly IdentityService identityService;

        public NotificationService(OurTraceDbContext dbContext,
            IMapper automapper)
        {
            this.dbContext = dbContext;
            this.automapper = automapper;
            this.identityService = new IdentityService(dbContext);
        }
        public async Task AddNotificationToUserAsync(NotificationServiceModel notificationModel)
        {
            if (AllStringPropertiesAreFilled(notificationModel))
            {
                var user = await this.identityService
                    .GetUserByName(notificationModel.Username)
                    .SingleOrDefaultAsync();

                if (user != null)
                {
                    var notification = automapper.Map<Notification>(notificationModel);
                    notification.User = user;

                    await this.dbContext.Notifications.AddAsync(notification);
                    await this.dbContext.SaveChangesAsync();
                }
            }
        }

        public async Task<ICollection<NotificationViewModel>> GetUserNotificationsAsync(string username, int count, bool orderedDescending)
        {
            var user = await this.identityService
                .GetUserByName(username)
                .Include(x => x.Notifications)
                .SingleOrDefaultAsync();

            if (user != null && user.Notifications != null && user.Notifications.Count > 0)
            {
                var userNotifications = user.Notifications.Take(count);
                if (orderedDescending)
                {
                    userNotifications = userNotifications.OrderByDescending(x => x.DateIssued).ToList();
                }
                var notifications = automapper.Map<ICollection<NotificationViewModel>>(userNotifications);
                return notifications;
            }
            return new List<NotificationViewModel>();
        }

        private bool AllStringPropertiesAreFilled(object model)
        {
            var properties = model.GetType().GetProperties().Where(x => x.PropertyType == typeof(string));
            bool result = true;
            foreach (var property in properties)
            {
                if (string.IsNullOrEmpty((string)property.GetValue(model, null)))
                    result = false;
            }
            return result;
        }
        public async Task MarkAllNotificationsAsSeenAsync(string username)
        {
            var user = await identityService.GetUserByName(username)
                .Include(x => x.Notifications)
                .SingleOrDefaultAsync();

            if (user != null)
            {
                foreach (var notification in user.Notifications.Where(x => x.Seen == false))
                {
                    notification.Seen = true;
                }
                await this.dbContext.SaveChangesAsync();
            }
        }
        public async Task<NotificationViewModel> MarkNotificationAsSeenAsync(string id)
        {
            var notification = await this.dbContext.Notifications
                .SingleOrDefaultAsync(x => x.Id == id);

            if (notification != null)
            {
                if (notification.Seen == false)
                {
                    notification.Seen = true;
                    await this.dbContext.SaveChangesAsync();
                }
                
                return automapper.Map<NotificationViewModel>(notification);
            }
            return null;
        }
    }
}
