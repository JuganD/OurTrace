using OurTrace.Data.Identity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OurTrace.Data.Models
{
    public class Notification
    {
        public Notification()
        {
            this.Id = Guid.NewGuid().ToString();
            this.DateIssued = DateTime.UtcNow;
        }
        public string Id { get; set; }
        public OurTraceUser User { get; set; }
        public string UserId { get; set; }
        public DateTime DateIssued { get; set; }
        public string Content { get; set; }
        public bool Seen { get; set; }

        // URL building
        public string ElementId { get; set; } // Id of the notificated element
        public string Controller { get; set; } // Controller of the notificated element
        public string Action { get; set; } // Action of the notificated element
    }
}
