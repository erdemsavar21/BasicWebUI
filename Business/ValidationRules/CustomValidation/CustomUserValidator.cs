using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.ServiceAdapters.AspIdentity.Model;
using Microsoft.AspNetCore.Identity;

namespace Business.ValidationRules.CustomValidation
{
    public class CustomUserValidator : IUserValidator<AppIdentityUser>
    {
        public CustomUserValidator()
        {
        } 

        public Task<IdentityResult> ValidateAsync(UserManager<AppIdentityUser> manager, AppIdentityUser user)
        {
            List<IdentityError> errors = new List<IdentityError>();
            string[] Digits = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };


            foreach (var digit in Digits)
            {
                if (user.UserName[0].ToString()==digit)
                {
                    errors.Add(new IdentityError() { Code = "UserNameBeginWithDigits", Description = "User name could not begin with digit numbers" });

                }
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
