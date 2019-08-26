using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OurTrace.App.Models.InputModels.Message;
using OurTrace.App.Models.ViewModels.Message;
using OurTrace.Services.Abstraction;

namespace OurTrace.App.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private readonly IMessageService messageService;
        private readonly IRelationsService relationsService;
        private readonly INotificationService notificationService;

        public MessageController(IMessageService messageService,
            IRelationsService relationsService,
            INotificationService notificationService)
        {
            this.messageService = messageService;
            this.relationsService = relationsService;
            this.notificationService = notificationService;
        }

        [HttpGet("Message/{name}")]
        public async Task<IActionResult> Chat(string name)
        {
            // Authorize users
            if (await this.relationsService.AreFriendsWithAsync(this.User.Identity.Name, name))
            {
                var viewModel = new MessageCollectionViewModel();
                viewModel.Recipient = name;
                viewModel.Messages = await this.messageService
                    .GetMessagesAsync(this.User.Identity.Name, name);

                return View(viewModel);
            }
            return Unauthorized();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> SendMessage(SendMessageInputModel model)
        {
            if (ModelState.IsValid)
            {
                if (await this.relationsService.AreFriendsWithAsync(this.User.Identity.Name, model.Recipient))
                {
                    await this.messageService.SendMessageAsync(this.User.Identity.Name, model.Recipient, model.Content);
                    await this.notificationService.AddNotificationToUserAsync(new Services.Models.NotificationServiceModel()
                    {
                        Action = "/",
                        Controller = "Message",
                        ElementId = this.User.Identity.Name,
                        Username = model.Recipient,
                        Content = this.User.Identity.Name + " said: " + model.Content
                    });
                }
            }
            return RedirectToAction("Chat", new { name = model.Recipient });
        }
    }
}