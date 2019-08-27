using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OurTrace.App.Models.ViewModels.Message;
using OurTrace.Data;
using OurTrace.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OurTrace.Services
{
    public class MessageService : IMessageService
    {
        private readonly OurTraceDbContext dbContext;
        private readonly IMapper automapper;
        private readonly IRelationsService relationsService;
        private readonly IdentityService identityService;

        public MessageService(OurTraceDbContext dbContext,
            IMapper automapper,
            IRelationsService relationsService)
        {
            this.dbContext = dbContext;
            this.automapper = automapper;
            this.relationsService = relationsService;
            this.identityService = new IdentityService(dbContext);
        }

        public async Task<ICollection<string>> GetAllUsernamesOfMessageIssuersAsync(string username)
        {
            var recipients = await this.dbContext.Messages
                .Where(x => x.Sender.UserName == username)
                .Select(x => x.Recipient.UserName)
                .ToListAsync();

            var senders = await this.dbContext.Messages
                .Where(x => x.Recipient.UserName == username)
                .Select(x => x.Sender.UserName)
                .ToListAsync();

            var friends = await this.relationsService.GetFriendsUsernamesAsync(username);

            return recipients.Union(senders).Union(friends).ToList();
        }

        public async Task<ICollection<MessageViewModel>> GetMessagesAsync(string sender, string recipient)
        {
            var messages = await this.dbContext.Messages
                .Where(x => (x.Sender.UserName == sender &&
                          x.Recipient.UserName == recipient) ||
                          (x.Sender.UserName == recipient &&
                          x.Recipient.UserName == sender))
                .Include(x => x.Sender)
                .OrderBy(x => x.CreatedOn)
                .ToListAsync();

            var viewModel = automapper.Map<ICollection<MessageViewModel>>(messages);

            return viewModel;
        }

        public async Task SendMessageAsync(string sender, string recipient, string content)
        {
            var senderUser = await this.identityService.GetUserByName(sender)
                .SingleOrDefaultAsync();
            var recipientUser = await this.identityService.GetUserByName(recipient)
                .SingleOrDefaultAsync();

            if (senderUser != null && recipientUser != null && content != null)
            {
                await this.dbContext.Messages.AddAsync(new Data.Models.Message()
                {
                    Content = content,
                    Recipient = recipientUser,
                    Sender = senderUser
                });
                await this.dbContext.SaveChangesAsync();
            }
        }
    }
}
