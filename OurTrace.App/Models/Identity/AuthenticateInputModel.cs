using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OurTrace.App.Models.Identity
{
    public class AuthenticateInputModel
    {
        public AuthenticateInputModel()
        {
            Login = new LoginInputModel();
            Register = new RegisterInputModel();
        }
        public LoginInputModel Login { get; set; }
        public RegisterInputModel Register { get; set; }
    }
}
