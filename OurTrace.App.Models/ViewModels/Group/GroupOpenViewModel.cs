using OurTrace.App.Models.ViewModels.Post;
using System.Collections.Generic;

namespace OurTrace.App.Models.ViewModels.Group
{
    public class GroupOpenViewModel
    {
        public string Name { get; set; }
        public int MembersCount { get; set; }
        public string CreatedOn { get; set; } 
        public string WallId { get; set; }
        public ICollection<PostViewModel> Posts { get; set; }
    }
}
