using AutoMapper.Configuration.Annotations;
using OurTrace.App.Models.ViewModels.Post;
using OurTrace.Data.Identity.Models;
using System.Collections.Generic;

namespace OurTrace.App.Models.ViewModels.Profile
{
    public class ProfileViewModel
    {
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Country { get; set; }
        public UserSex Sex { get; set; }
        public int Years { get; set; }
        public int FriendsCount { get; set; }
        public string JoinedOn { get; set; } // TODO
        public string WallId { get; set; }
        public int Following { get; set; }
        public int Followers { get; set; }
        [Ignore]
        public bool AreFriends { get; set; }
        [Ignore]
        public bool PendingFriendship { get; set; }
        [Ignore]
        public bool IsFollowing { get; set; }
        [Ignore]
        public bool IsHimself { get; set; }
        public ICollection<PostViewModel> Posts { get; set; }
        
    }
}
