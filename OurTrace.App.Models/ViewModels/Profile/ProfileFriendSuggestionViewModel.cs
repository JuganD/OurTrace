using AutoMapper.Configuration.Annotations;

namespace OurTrace.App.Models.ViewModels.Profile
{
    public class ProfileFriendSuggestionViewModel
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        [Ignore]
        public string HisFriendUsername { get; set; }
    }
}
