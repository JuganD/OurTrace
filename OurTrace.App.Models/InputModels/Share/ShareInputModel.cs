using OurTrace.App.Models.InputModels.Posts;
using System.ComponentModel.DataAnnotations;

namespace OurTrace.App.Models.InputModels.Share
{
    public class ShareInputModel
    {
        public string PostId { get; set; }
        [Required]
        public ShareLocation ShareLocationType { get; set; }
        public string ShareLocation { get; set; }
        public CreatePostInputModel PostModel { get; set; }
    }
    public enum ShareLocation
    {
        MyWall = 1,
        FriendWall = 2,
        GroupWall = 3
    }
}
