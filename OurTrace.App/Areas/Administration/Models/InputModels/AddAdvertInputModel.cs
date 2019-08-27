using OurTrace.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace OurTrace.App.Areas.Administration.Models.InputModels
{
    public class AddAdvertInputModel
    {
        [Required]
        public AdvertType Type { get; set; }

        [Required]
        public string IssuerName { get; set; }

        public string Content { get; set; }

        [Range(0, int.MaxValue)]
        public int ViewsLeft { get; set; }
    }
}
