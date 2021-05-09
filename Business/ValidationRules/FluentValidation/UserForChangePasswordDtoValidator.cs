using System;
using Entities.Dtos;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class UserForChangePasswordDtoValidator : AbstractValidator<UserForChangePasswordDto>
    {
        public UserForChangePasswordDtoValidator()
        {

            
            RuleFor(p => p.PasswordOld).NotEmpty().WithMessage("Old Password can not be empty");
            RuleFor(p => p.PasswordNew).NotEmpty().WithMessage("Password can not be empty");
            RuleFor(p => p.PasswordNew).MinimumLength(4).WithMessage("Password muss mindestens 4 Zeichen lang sein");
            RuleFor(p => p.PasswordNew).Equal(p => p.ConfirmPassword).WithMessage("Password muss ConfirmPassword gleich sein.");
        }


    }
}
