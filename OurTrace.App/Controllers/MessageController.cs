using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OurTrace.App.Models.ViewModels.Message;
using OurTrace.Services.Abstraction;

namespace OurTrace.App.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private readonly IMessageService messageService;
        private readonly IRelationsService relationsService;
        private readonly IUserService userService;

        public MessageController(IMessageService messageService,
            IRelationsService relationsService,
            IUserService userService)
        {
            this.messageService = messageService;
            this.relationsService = relationsService;
            this.userService = userService;
        }

        [HttpGet]
        [Route("Message/{name?}")]
        public async Task<IActionResult> Chat(string name)
        {
            bool ignoreQuery = false;
            if (name == this.User.Identity.Name || !await this.userService.UserExistsAsync(name))
            {
                ignoreQuery = true;
            }

            // Authorize users
            var viewModel = new MessageCollectionViewModel();
            viewModel.OtherFriendsMessages = await this.messageService.GetAllUsernamesOfMessageIssuersAsync(this.User.Identity.Name);

            if (ignoreQuery)
            {
                if (viewModel.OtherFriendsMessages.Any())
                    name = viewModel.OtherFriendsMessages.First();
                else
                    return View("Error", "No friends avaible to chat with! Consider adding some!");
            }

            viewModel.Recipient = name;
            viewModel.AreFriends = await this.relationsService.AreFriendsWithAsync(this.User.Identity.Name, name);
            viewModel.Messages = await this.messageService
                .GetMessagesAsync(this.User.Identity.Name, name);

            return View(viewModel);
        }
    }
}