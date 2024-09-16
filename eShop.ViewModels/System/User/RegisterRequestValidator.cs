using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.ViewModels.System.User
{
    public class RegisterRequestValidator :AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator() 
        {
            RuleFor(x=>x.FirstName).NotEmpty().WithMessage("First name must require");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name must require");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email must require");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Email must correct format");

        }
    }
}
