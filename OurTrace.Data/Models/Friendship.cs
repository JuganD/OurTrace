using OurTrace.Data.Identity.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace OurTrace.Data.Models
{
    public class Friendship
    {
        public Friendship()
        {
            this.Id = Guid.NewGuid().ToString();
            this.IssuedOn = DateTime.UtcNow;
        }
        [Key]
        public string Id { get; set; }

        public string SenderId { get; set; }
        public OurTraceUser Sender { get; set; }

        public string RecipientId { get; set; }
        public OurTraceUser Recipient { get; set; }

        public DateTime IssuedOn { get; set; }
        // TODO: don't forget to change this on accepting
        public DateTime? AcceptedOn { get; set; }
    }
}
