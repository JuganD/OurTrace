using OurTrace.Data.Identity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OurTrace.Data.Models
{
    public class UserGroup
    {
        public string UserId { get; set; }
        public OurTraceUser User { get; set; }
        public string GroupId { get; set; }
        public Group Group { get; set; }
    }
}
