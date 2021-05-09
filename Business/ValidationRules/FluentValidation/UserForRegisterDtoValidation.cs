using System;
using Entities.Dtos;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class UserForRegisterDtoValidation : AbstractValidator<UserForRegisterDto>
    {
        public UserForRegisterDtoValidation()
        {
            RuleFor(p => p.UserName).NotEmpty().WithMessage("Username con not be empty");
            RuleFor(p => p.Email).NotEmpty().WithMessage("Email can not be empty").EmailAddress();
            RuleFor(p => p.Password).NotEmpty().WithMessage("Password can not be empty");
            RuleFor(p => p.Password).MinimumLength(4).WithMessage("Password muss mindestens 4 Zeichen lang sein");
        }
    }
}
