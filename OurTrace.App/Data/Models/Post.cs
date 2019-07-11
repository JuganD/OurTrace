using OurTrace.App.Data.Identity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OurTrace.App.Data.Models
{
    public class Post
    {
        public Post()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Comments = new List<Comment>();
            this.CreatedOn = DateTime.UtcNow;
        }
        [Key]
        public string Id { get; set; }
        public string MediaUrl { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? EditedOn { get; set; } // can be null if never edited
        public int Likes { get; set; }
        public int Shares { get; set; }
        public PostMimeType MimeType { get; set; }
        public PostVisibilityType VisibilityType { get; set; }

        public string UserId { get; set; }
        public OurTraceUser User { get; set; }

        public string LocationId { get; set; }
        public Wall Location { get; set; }

        public ICollection<Comment> Comments { get; set; }

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
}
