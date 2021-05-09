using System;
using Entities.Dtos;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class UserForResetPasswordDtoValidator : AbstractValidator<UserForResetPasswordDto>
    {
        public UserForResetPasswordDtoValidator()
        {

            RuleFor(p => p.UserName).NotEmpty().WithMessage("UserName can not be empty");
            RuleFor(p => p.Code).NotEmpty().WithMessage("Code can not be empty");
            RuleFor(p => p.Password).NotEmpty().WithMessage("Password can not be empty");
            RuleFor(p => p.Password).MinimumLength(4).WithMessage("Password muss mindestens 4 Zeichen lang sein");
            RuleFor(p => p.Password).Equal(p => p.ConfirmPassword).WithMessage("Password muss ConfirmPassword gleich sein.");
        }


    }
}
