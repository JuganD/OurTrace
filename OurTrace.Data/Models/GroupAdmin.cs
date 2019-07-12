using OurTrace.Data.Identity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OurTrace.Data.Models
{
    public class GroupAdmin
    {
        public GroupAdmin()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        [Key]
        public string Id { get; set; }

        public string UserId { get; set; }
        public OurTraceUser User { get; set; }

        public string GroupId { get; set; }
        public Group Group { get; set; }

        public GroupAdminType AdminType { get; set; }
    }
    public enum GroupAdminType
    {
        Moderator = 1,
        Admin = 2
    }
}
