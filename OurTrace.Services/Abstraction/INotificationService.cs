using OurTrace.App.Models.ViewModels.Notification;
using OurTrace.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OurTrace.Services.Abstraction
{
    public interface INotificationService
    {
        Task AddNotificationToUserAsync(NotificationServiceModel notificationModel);
        Task<NotificationViewModel> MarkNotificationAsSeenAndReturnItAsync(string id);
        Task MarkAllNotificationsAsSeenAsync(string username);
        Task<ICollection<NotificationViewModel>> GetUserNotificationsAsync(string username, int count, bool orderedDescending);
    }
}
