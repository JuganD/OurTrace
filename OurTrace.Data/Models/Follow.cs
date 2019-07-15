using OurTrace.Data.Identity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OurTrace.Data.Models
{
    public class Follow
    {
        public Follow()
        {
            this.Id = Guid.NewGuid().ToString();
            this.FollowedOn = DateTime.UtcNow;
        }
        [Key]
        public string Id { get; set; }

        public string SenderId { get; set; }
        public OurTraceUser Sender { get; set; }

        public string RecipientId { get; set; }
        public OurTraceUser Recipient { get; set; }

        public DateTime FollowedOn { get; set; }
    }
}
