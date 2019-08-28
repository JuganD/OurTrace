using OurTrace.App.Models.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OurTrace.App.Models
{
    public class NewsfeedDataModel
    {
        public string Username { get; set; }
        public int Location { get; set; }
        public NewsfeedViewModel Model { get; set; }
    }
}
