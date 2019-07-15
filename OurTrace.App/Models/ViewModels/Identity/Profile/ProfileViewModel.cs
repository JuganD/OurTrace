using OurTrace.App.Models.ViewModels.Posts;
using System.Collections.Generic;

namespace OurTrace.App.Models.ViewModels.Identity.Profile
{
    public class ProfileViewModel
    {
        public string FullName { get; set; }
        public string Username { get; set; }
        public int Years { get; set; }
        public string JoinedOn { get; set; } // TODO
        public int Following { get; set; }
        public int Followers { get; set; }
        public ICollection<PostViewModel> Posts { get; set; }
        
    }
}
