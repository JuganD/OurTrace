using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using OurTrace.App.Models.ViewModels.Message;
using OurTrace.Services.Abstraction;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace OurTrace.App.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IRelationsService relationsService;
        private readonly INotificationService notificationService;
        private readonly IMessageService messageService;
        private readonly static ConcurrentDictionary<string, IClientProxy> ConnectedUsers
            = new ConcurrentDictionary<string, IClientProxy>();

        public ChatHub(IRelationsService relationsService,
            INotificationService notificationService,
            IMessageService messageService)
        {
            this.relationsService = relationsService;
            this.notificationService = notificationService;
            this.messageService = messageService;
        }
        public async Task Send(string username, string message)
        {
            if (await this.relationsService.AreFriendsWithAsync(this.Context.User.Identity.Name, username))
            {
                await this.messageService.SendMessageAsync(this.Context.User.Identity.Name, username, message);
                await this.notificationService.AddNotificationToUserAsync(new Services.Models.NotificationServiceModel()
                {
                    Action = "/",
                    Controller = "Message",
                    ElementId = this.Context.User.Identity.Name,
                    Username = username,
                    Content = this.Context.User.Identity.Name + " said: " + message
                });
                IClientProxy userproxy;
                var userIdFetchResult = ConnectedUsers.TryGetValue(username, out userproxy);
                if (userIdFetchResult)
                {
                    await userproxy.SendAsync("NewMessage", new MessageViewModel() {Sender = this.Context.User.Identity.Name, Content = message });
                }
            }
        }
        public async override Task OnConnectedAsync()
        {
            ConnectedUsers.AddOrUpdate(this.Context.User.Identity.Name
                , this.Clients.Caller,
                (key, oldValue) => this.Clients.Caller);

            await base.OnConnectedAsync();
        }
        public async override Task OnDisconnectedAsync(Exception exception)
        {
            ConnectedUsers.TryRemove(this.Context.User.Identity.Name, out _);

            await base.OnDisconnectedAsync(exception);
        }
    }
}
