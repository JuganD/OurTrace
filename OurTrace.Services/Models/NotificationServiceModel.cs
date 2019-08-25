using AutoMapper.Configuration.Annotations;

namespace OurTrace.Services.Models
{
    public class NotificationServiceModel
    {
        [Ignore]
        public string Username { get; set; }
        public string Content { get; set; }
        public string ElementId { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}
