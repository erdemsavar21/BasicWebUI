using System;
using Entities.Dtos;

namespace WebUI.Models.Account
{
    public class AccountViewModel
    {
        public AccountViewModel()
        {
        }

        public UserForRegisterDto userForRegisterDto { get; set; }
        public UserForLoginDto userForLoginDto { get; set; }
        public UserForForgotPasswordDto userForForgotPasswordDto { get; set; }
        public UserForResetPasswordDto userForResetPasswordDto { get; set; }
       
    }
}
