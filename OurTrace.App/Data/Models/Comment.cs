﻿using OurTrace.App.Data.Identity.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace OurTrace.App.Data.Models
{
    public class Comment
    {
        public Comment()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CreatedOn = DateTime.UtcNow;
        }
        [Key]
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? EditedOn { get; set; } // can be null if never edited

        public string UserId { get; set; }
        public OurTraceUser User { get; set; }

        public string PostId { get; set; }
        public Post Post { get; set; }
    }
}