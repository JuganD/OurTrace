using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OurTrace.Services.Abstraction;

namespace OurTrace.App.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly INotificationService notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }
        public async Task<IActionResult> Open(string id)
        {
            var notification = await this.notificationService
                .MarkNotificationAsSeenAsync(id);
            if (notification != null)
            {
                var redirectPartial = Url.Action(notification.Action, notification.Controller);
                // Because it wants parameters
                if (string.IsNullOrEmpty(redirectPartial))
                {
                    redirectPartial = "/" + notification.Controller + "/" + notification.Action;
                    
                }
                var redirectLocation = redirectPartial + "/" + notification.ElementId;
                redirectLocation = Regex.Replace(redirectLocation, @"/+", @"/");
                return Redirect(redirectLocation);
            }
            return NotFound();
        }
        public async Task<IActionResult> Last50()
        {
            return View(await this.notificationService
                .GetUserNotificationsAsync(this.User.Identity.Name, 50, true));
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> MarkAllSeen(bool redir)
        {
            await this.notificationService.MarkAllNotificationsAsSeenAsync(this.User.Identity.Name);
            if (redir)
            {
                return RedirectToAction("Last50");
            }
            else
            {
                return StatusCode(200, "Ok");
            }
        }
    }
}