using OurTrace.App.Models.ViewModels.Comments;
using OurTrace.Data.Models;
using System;
using System.Collections.Generic;

namespace OurTrace.App.Models.ViewModels.Posts
{
    public class PostViewModel
    {
        public string MediaUrl { get; set; }
        public string Content { get; set; }
        public string CreatedOn { get; set; }
        public string EditedOn { get; set; }
        public int Likes { get; set; }
        public int Shares { get; set; }
        public PostMimeType MimeType { get; set; }
        public PostVisibilityType VisibilityType { get; set; }
        public string Creator { get; set; }
        public ICollection<CommentViewModel> Comments { get; set; }
    }
}
