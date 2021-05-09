using System;
using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace Entities.Dtos
{
    public class UserForLoginDto:IDto
    {
        public UserForLoginDto()
        {
        }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email is not the correct format")]
        public string Email { get; set; }
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [MinLength(4,ErrorMessage = "Password muss mindestens 4 Zeichen lang sein")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
