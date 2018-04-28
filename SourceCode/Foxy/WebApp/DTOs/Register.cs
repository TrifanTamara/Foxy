using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.DTOs_Validators;

namespace WebApp.DTOs
{
    [FluentValidation.Attributes.Validator(typeof(RegisterValidator))]
    public class Register
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
