using OurTrace.App.Models.Attributes.Validation;
using OurTrace.Data.Identity.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace OurTrace.App.Models.InputModels.Identity
{
    public class RegisterInputModel
    {
        [Required]
        [Display(Name = "Username")]
        [NoSpecialSymbols]
        [StringLength(maximumLength: 20, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        [NoSpecialSymbols]
        [StringLength(maximumLength: 50, MinimumLength = 3)]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Country")]
        [NoSpecialSymbols]
        [IsValidCountry(ErrorMessage = "Not a valid country.")]
        public string Country { get; set; }

        [Required]
        [Display(Name = "Sex")]
        [StringRange(AllowableValues = new string[] { "Male", "Female" }, ErrorMessage = "Please select sex from the dropdown")]
        public UserSex Sex { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime? BirthDate { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [IsTrue(ErrorMessage = "You need to agree with the terms of service!")]
        public bool TermsOfServiceAgreement { get; set; }
    }
}