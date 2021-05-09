using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.ServiceAdapters.AspIdentity.Model;
using Microsoft.AspNetCore.Identity;

namespace Business.ValidationRules.CustomValidation
{
    public class CustomPasswordValidator : IPasswordValidator<AppIdentityUser>
    {
        public CustomPasswordValidator()
        {
        }

        public Task<IdentityResult> ValidateAsync(UserManager<AppIdentityUser> manager, AppIdentityUser user, string password)
        {
            List<IdentityError> errors = new List<IdentityError>();

            //if (password.ToLower().Contains(user.UserName.ToLower()))
            //{
            //    errors.Add(new IdentityError() { Code = "PasswordContainesUserName", Description = "Sifre alani kullanici adi icermemeli." });
            //}

            if (password.ToLower().Contains("1234"))
            {
                errors.Add(new IdentityError() { Code = "PasswordContaines1234", Description = "Sifre alani ardasik sayi icermemeli." });
            }


            if (errors.Count == 0)
            {
                return Task.FromResult(IdentityResult.Success);
            }
            else
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }
        }
    }
}
