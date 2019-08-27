using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OurTrace.Data.Models
{
    public class Advert
    {
        [Key]
        public string Id { get; set; }
        public AdvertType Type { get; set; }
        public string IssuerName { get; set; }
        public string Content { get; set; }
        public int ViewsLeft { get; set; }
    }
    public enum AdvertType
    {
        User = 1,
        Group = 2
    }
}
