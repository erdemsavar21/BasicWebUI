using System;
using Entities.Dtos;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class UserForForgotPasswordDtoValidator : AbstractValidator<UserForForgotPasswordDto>
    {
        public UserForForgotPasswordDtoValidator()
        {

            RuleFor(p => p.Email).NotEmpty().WithMessage("Email can not be empty").EmailAddress();

        }
    }
}
