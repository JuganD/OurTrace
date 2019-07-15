using OurTrace.Data.Identity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OurTrace.Data.Models
{
    public class CommentLike
    {
        public CommentLike()
        {
            this.Id = Guid.NewGuid().ToString();
            this.LikedOn = DateTime.UtcNow;
        }
        public string Id { get; set; }
        public string CommentId { get; set; }
        public Comment Comment { get; set; }

        public string UserId { get; set; }
        public OurTraceUser User { get; set; }

        public DateTime LikedOn { get; set; }
    }
}
