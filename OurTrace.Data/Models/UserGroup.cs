using OurTrace.Data.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OurTrace.Data.Models
{
    public class UserGroup
    {
        public string GroupId { get; set; }
        public Group Group { get; set; }
        public string UserId { get; set; }
        public OurTraceUser User { get; set; }
    }
}
