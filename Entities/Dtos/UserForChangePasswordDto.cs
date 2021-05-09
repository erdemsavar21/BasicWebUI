using System;
using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace Entities.Dtos
{
    public class UserForChangePasswordDto : IDto
    {
        public UserForChangePasswordDto()
        {
        }


        [Display(Name = "PasswordOld")]
        [DataType(DataType.Password)]
        public string PasswordOld { get; set; }

        [Display(Name = "PasswordNew")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Password muss mindestens 4 Zeichen lang sein")]
        public string PasswordNew { get; set; }

        [Display(Name = "ConfirmPassword")]
        [DataType(DataType.Password)]
        
        public string ConfirmPassword { get; set; }

      
    }
}
