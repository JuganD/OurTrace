using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace OurTrace.Data.Identity.Models
{
    public class OurTraceRole : IdentityRole<string>
    {
        public OurTraceRole() : base()
        {

        }
        public OurTraceRole(string roleName) : base(roleName)
        {

        }
    }
}
