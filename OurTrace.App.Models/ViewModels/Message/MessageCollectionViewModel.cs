using System;
using System.Collections.Generic;
using System.Text;

namespace OurTrace.App.Models.ViewModels.Message
{
    public class MessageCollectionViewModel
    {
        public MessageCollectionViewModel()
        {
            this.Messages = new List<MessageViewModel>();
        }
        public string Recipient { get; set; }
        public ICollection<string> OtherFriendsMessages { get; set; }
        public ICollection<MessageViewModel> Messages { get; set; }
    }
}
