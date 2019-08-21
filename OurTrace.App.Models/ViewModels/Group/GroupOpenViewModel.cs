using AutoMapper.Configuration.Annotations;
using OurTrace.App.Models.ViewModels.Post;
using System.Collections.Generic;

namespace OurTrace.App.Models.ViewModels.Group
{
    public class GroupOpenViewModel
    {
        public string Name { get; set; }
        public string CreatedOn { get; set; } 
        public string WallId { get; set; }
        [Ignore]
        public bool IsUserMemberOfGroup { get; set; } // view-pointing variable (INCLUDES PENDING USERS)
        [Ignore]
        public bool IsUserConfirmed { get; set; } // view-pointing variable
        [Ignore]
        public bool IsAdministrator { get; set; } // view-pointing variable
        [Ignore]
        public bool IsModerator { get; set; } // view-pointing variable
        [Ignore]
        public int GroupRank { get; set; } // view-pointing variable
        public ICollection<PostViewModel> Posts { get; set; }
        public ICollection<GroupMemberViewModel> JoinRequests { get; set; }
        public ICollection<GroupMemberViewModel> Members { get; set; }
    }
}
