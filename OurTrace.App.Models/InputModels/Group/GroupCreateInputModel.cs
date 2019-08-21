using OurTrace.App.Models.Attributes.Validation;
using System.ComponentModel.DataAnnotations;

namespace OurTrace.App.Models.InputModels.Group
{
    public class GroupCreateInputModel
    {
        [Required]
        [NoSpecialSymbols]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
