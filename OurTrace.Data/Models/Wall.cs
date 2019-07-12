using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OurTrace.Data.Models
{
    public class Wall
    {
        public Wall()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Posts = new List<Post>();
        }
        [Key]
        public string Id { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}
