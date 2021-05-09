using System;
using Business.ServiceAdapters.AspIdentity.Model;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Jwt;
using Entities.Dtos;
using Microsoft.AspNetCore.Authentication;

namespace Business.Abstract
{
    public interface IAuthService
    {
        IDataResult<User> Register(UserForRegisterDto userForRegisterDto);
        IDataResult<User> Login(UserForLoginDto userForLoginDto);
        void SignOut();
        IResult SendPasswordResetMail(UserForForgotPasswordDto userForForgotPasswordDto);
        IResult UserExists(string userName);
        IResult ResetPassword(UserForResetPasswordDto userForResetPasswordDto);
        IResult ConfirmEmail(string userName, string code);
        // IResult ChangePassword(string userName, UserForChangePasswordDto userForChangePasswordDto);
        // IDataResult<AccessToken> CreateAccessToken(User user);
        AuthenticationProperties GetAuthenticationProperties(string redirectUrl,string provider);
        IResult LoginWithExternalAccount();
    }
}
