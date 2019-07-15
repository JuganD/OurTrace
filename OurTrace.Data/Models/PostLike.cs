using OurTrace.Data.Identity.Models;
using System;

namespace OurTrace.Data.Models
{
    public class PostLike
    {
        public PostLike()
        {
            this.Id = Guid.NewGuid().ToString();
            this.LikedOn = DateTime.UtcNow;
        }
        public string Id { get; set; }
        public string PostId { get; set; }
        public Post Post { get; set; }

        public string UserId { get; set; }
        public OurTraceUser User { get; set; }

        public DateTime LikedOn { get; set; }
    }
}