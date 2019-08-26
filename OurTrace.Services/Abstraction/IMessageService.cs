using OurTrace.App.Models.ViewModels.Message;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OurTrace.Services.Abstraction
{
    public interface IMessageService
    {
        Task<ICollection<MessageViewModel>> GetMessagesAsync(string sender, string recipient);
        Task SendMessageAsync(string sender, string recipient, string content);
    }
}
