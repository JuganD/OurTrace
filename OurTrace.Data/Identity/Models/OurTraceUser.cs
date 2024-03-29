﻿using Microsoft.AspNetCore.Identity;
using OurTrace.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OurTrace.Data.Identity.Models
{
    public class OurTraceUser : IdentityUser<string>
    {
        public OurTraceUser() : base()
        {
            this.Posts = new List<Post>();
            this.Comments = new List<Comment>();
            this.SentFriendships = new List<Friendship>();
            this.ReceivedFriendships = new List<Friendship>();
            this.Groups = new List<Group>();
            this.Following = new List<Follow>();
            this.Followers = new List<Follow>();
            this.Notifications = new List<Notification>();
            this.Wall = new Wall();
            this.JoinedOn = DateTime.UtcNow;
        }
        public string FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Country { get; set; }
        public UserSex Sex { get; set; }

        public DateTime JoinedOn { get; set; }

        public string WallId { get; set; }
        public Wall Wall { get; set; }

        public ICollection<Post> Posts { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Friendship> SentFriendships { get; set; }
        public ICollection<Friendship> ReceivedFriendships { get; set; }
        public ICollection<Group> Groups { get; set; }
        public ICollection<Follow> Following { get; set; }
        public ICollection<Follow> Followers { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }

    // There are only two genders OKAY ? :@
    public enum UserSex
    {
        Male = 1,
        Female = 2
    }
}
