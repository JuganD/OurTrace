using System.ComponentModel.DataAnnotations;

namespace OurTrace.App.Models.Attributes.Validation
{
    public class NoSpecialSymbolsAttribute : RegularExpressionAttribute
    {
        public NoSpecialSymbolsAttribute(string errormessage = null) 
            : base(@"^[\w\s-]+$")
        {
            if (errormessage == null)
            {
                this.ErrorMessage = @"Characters ^<>,?;:'()!~%\#/*"" are not allowed.";
            }
        }
    }
}
