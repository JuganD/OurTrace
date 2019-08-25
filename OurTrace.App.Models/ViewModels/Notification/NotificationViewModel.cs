using System;

namespace OurTrace.App.Models.ViewModels.Notification
{
    public class NotificationViewModel
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public string ElementId { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool Seen { get; set; }
        public string DateIssued { get; set; }
    }
}
