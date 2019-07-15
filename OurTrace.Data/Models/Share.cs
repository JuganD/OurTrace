using OurTrace.Data.Identity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OurTrace.Data.Models
{
    public class Share
    {
        public Share()
        {
            this.Id = Guid.NewGuid().ToString();
            this.SharedOn = DateTime.UtcNow;
        }
        [Key]
        public string Id { get; set; }

        public string UserId { get; set; }
        public OurTraceUser User { get; set; }

        public DateTime SharedOn { get; set; }

        public string PostId { get; set; }
        public Post Post { get; set; }
    }
}
