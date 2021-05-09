using System;
using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace Entities.Dtos
{
    public class UserForForgotPasswordDto : IDto
    {
        public UserForForgotPasswordDto()
        {
        }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email is not the correct format")]
        public string Email { get; set; }
    }
}
