using System;
using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace Entities.Dtos
{
    public class UserForResetPasswordDto : IDto
    {
        public UserForResetPasswordDto()
        {
        }

        
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Password muss mindestens 4 Zeichen lang sein")]
        public string Password { get; set; }
        [Display(Name = "ConfirmPassword")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
        public string UserName { get; set; }


    }
}
