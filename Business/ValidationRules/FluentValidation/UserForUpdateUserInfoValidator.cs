using System;
using Core.Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class UserForUpdateUserInfoValidator:AbstractValidator<User>
    {
        public UserForUpdateUserInfoValidator()
        {
            RuleFor(p => p.UserName).NotEmpty().WithMessage("Username con not be empty");
            RuleFor(p => p.Email).NotEmpty().WithMessage("Email con not be empty").EmailAddress();
            
        }
    }
}

