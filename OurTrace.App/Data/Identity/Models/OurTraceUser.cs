using Microsoft.AspNetCore.Identity;
using OurTrace.App.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OurTrace.App.Data.Identity.Models
{
    public class OurTraceUser : IdentityUser<string>
    {
        public OurTraceUser() : base()
        {
            this.Posts = new List<Post>();
            this.Comments = new List<Comment>();
            this.SentFriendships = new List<Friendship>();
            this.ReceivedFriendships = new List<Friendship>();
            this.Groups = new List<UserGroup>();
        }
        public string FullName { get; set; }
        public DateTime? BirthDate { get; set; }

        public string WallId { get; set; }
        public Wall Wall { get; set; }

        public ICollection<Post> Posts { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Friendship> SentFriendships { get; set; }
        public ICollection<Friendship> ReceivedFriendships { get; set; }
        public ICollection<UserGroup> Groups { get; set; }
    }
}
