using AutoMapper.Configuration.Annotations;
using OurTrace.App.Models.ViewModels.Comments;
using OurTrace.Data.Models;
using System;
using System.Collections.Generic;

namespace OurTrace.App.Models.ViewModels.Post
{
    public class PostViewModel
    {
        public PostViewModel()
        {
            this.Likes = new List<PostLikeViewModel>();
        }
        public string Id { get; set; }
        public string MediaUrl { get; set; }
        public bool IsImageOnFileSystem { get; set; }
        [Ignore]
        public bool IgnoreComments { get; set; }
        public string Content { get; set; }
        public string CreatedOn { get; set; }
        public string EditedOn { get; set; }
        public PostViewModel SharedPost { get; set; }
        public PostMimeType MimeType { get; set; }
        public PostVisibilityType VisibilityType { get; set; }
        public string Creator { get; set; }
        public ICollection<CommentViewModel> Comments { get; set; }
        public ICollection<PostLikeViewModel> Likes { get; set; }
        public ICollection<PostShareViewModel> Shares { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}
