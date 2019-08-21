using AutoMapper.Configuration.Annotations;

namespace OurTrace.App.Models.ViewModels.Group
{
    public class GroupWindowViewModel
    {
        public string Name { get; set; }
        public int Members { get; set; }
        public string Url { get; set; }
        [Ignore]
        public bool PendingJoin { get; set; }
    }
}
