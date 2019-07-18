using OurTrace.App.Models.Attributes.Validation;
using System.ComponentModel.DataAnnotations;

namespace OurTrace.App.Models.InputModels.Settings
{
    public class DeleteAccountInputModel
    {
        [Required]
        [IsTrue]
        public bool WarningState { get; set; }
    }
}
