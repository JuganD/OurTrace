using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OurTrace.App.Models.ViewModels.Comments
{
    public class CommentViewModel
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public string CreatedOn { get; set; }
        public string EditedOn { get; set; }
        public string Creator { get; set; }
    }
}
