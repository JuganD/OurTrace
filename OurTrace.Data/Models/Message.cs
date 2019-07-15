using OurTrace.Data.Identity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OurTrace.Data.Models
{
    public class Message
    {
        public Message()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CreatedOn = DateTime.UtcNow;
        }
        [Key]
        public string Id { get; set; }

        public string SenderId { get; set; }
        public OurTraceUser Sender { get; set; }

        public string RecipientId { get; set; }
        public OurTraceUser Recipient { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
