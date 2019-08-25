using Microsoft.AspNetCore.Mvc;
using OurTrace.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OurTrace.App.ViewComponents
{
    public class NotificationsViewComponent : ViewComponent
    {
        private readonly INotificationService notificationService;

        public NotificationsViewComponent(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await notificationService.GetUserNotificationsAsync(this.User.Identity.Name, 50, true));
        }
    }
}
