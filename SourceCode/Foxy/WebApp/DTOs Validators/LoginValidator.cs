using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using WebApp.DTOs;

namespace WebApp.DTOs_Validators
{
    public class LoginValidator : AbstractValidator<Login>
    {
        public LoginValidator()
        {
            RuleFor(usr => usr.Email).NotEmpty().EmailAddress();
            RuleFor(usr => usr.Password).Length(8, 20);
        }
    }
}
