using OurTrace.Data.Identity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OurTrace.Data.Models
{
    public class Post
    {
        public Post()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CreatedOn = DateTime.UtcNow;
            this.Tags = new List<string>();
            this.Likes = new List<PostLike>();
            this.Comments = new List<Comment>();
            this.Shares = new List<Share>();
        }
        [Key]
        public string Id { get; set; }
        public string MediaUrl { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? EditedOn { get; set; } // can be null if never edited
        public PostMimeType MimeType { get; set; }
        public PostVisibilityType VisibilityType { get; set; }
        public PostType Type { get; set; }

        public string UserId { get; set; }
        public OurTraceUser User { get; set; }

        public string LocationId { get; set; }
        public Wall Location { get; set; }

        public ICollection<PostLike> Likes { get; set; }
        public ICollection<Share> Shares { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public IEnumerable<string> Tags { get; set; }

    }
    public enum PostMimeType
    {
        Regular = 0, // default value - intended
        Picture = 1,
        ProfilePicture = 2,
        Video = 3,
    }
    public enum PostVisibilityType
    {
        FriendsOnly = 0,
        Public = 1,
        Private = 2
    }
    public enum PostType
    {
        Original = 0,
        Shared = 1
    }
}
