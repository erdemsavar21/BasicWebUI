using System;
using Microsoft.AspNetCore.Identity;

namespace Business.ValidationRules.CustomValidation
{
    public class CustomIdentityErrorDescriber : IdentityErrorDescriber
    {
        public CustomIdentityErrorDescriber()
        {
        }

        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError()
            {
                Code = "InvalidUsernName",
                Description = $"Das ist {userName} ungültiger Benutzer"
            };
        }


        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError()
            {
                Code = "DuplicateEmail",
                Description = $"Das email ( {email} ) ist Duplikat"
            };
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError()
            {
                Code = "DuplicateUserName",
                Description = $"Das userName ( {userName} ) ist Duplikat"
            };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError()
            {
                Code = "PasswordTooShort",
                Description = $"Password muss langer als {length} sein"
            };
        }
    }
}
