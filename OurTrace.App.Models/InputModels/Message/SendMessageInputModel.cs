using System.ComponentModel.DataAnnotations;

namespace OurTrace.App.Models.InputModels.Message
{
    public class SendMessageInputModel
    { 
        [Required]
        public string Recipient { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
