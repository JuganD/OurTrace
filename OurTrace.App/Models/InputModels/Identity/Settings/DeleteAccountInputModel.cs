using OurTrace.App.Attributes.Validation;
using System.ComponentModel.DataAnnotations;

namespace OurTrace.App.Models.InputModels.Identity.Settings
{
    public class DeleteAccountInputModel
    {
        [Required]
        [IsTrue]
        public bool WarningState { get; set; }
    }
}
