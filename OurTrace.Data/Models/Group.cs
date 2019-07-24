using OurTrace.Data.Identity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OurTrace.Data.Models
{
    public class Group
    {
        public Group()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CreatedOn = DateTime.UtcNow;
            this.Admins = new List<GroupAdmin>();
            this.Members = new List<UserGroup>();
            this.Wall = new Wall();
        }
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public string CreatorId { get; set; }
        public OurTraceUser Creator { get; set; }

        public string WallId { get; set; }
        public Wall Wall { get; set; }

        public string Url { get; set; }

        public DateTime CreatedOn { get; set; }

        public ICollection<GroupAdmin> Admins { get; set; }
        public ICollection<UserGroup> Members { get; set; }
    }
}
