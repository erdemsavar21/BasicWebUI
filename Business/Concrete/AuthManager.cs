using System;
using System.Text;
using Business.Abstract;
using Business.Constant;
using Business.ServiceAdapters.AspIdentity.Model;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using Core.Entities.Concrete;
using Core.Utilities.Mail;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.Jwt;
using Entities.Dtos;
using Microsoft.AspNetCore.Authentication;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private IUserService _userService;
        private IMailService _mailService;

        public AuthManager(IUserService userService,IMailService mailService)
        {
            _userService = userService;
            _mailService = mailService;
        }


        [LogAspect(typeof(DatabaseLogger))]
        [ValidationAspect(typeof(UserForLoginDtoValidation), Priority = 1)]
        //[TransactionScopeAspect]
        [PerformanceAspect(5)]
        public IDataResult<User> Login(UserForLoginDto userForLoginDto)
        {
            var user = _userService.FindByEmail(userForLoginDto.Email);

            if (user == null)
            {
                return new ErrorDataResult<User>(null, "Email oder Password invalid");
            }


            if (!_userService.IsEmailConfirmed(user))
            {
                return new ErrorDataResult<User>(user, "Confirm your Email..");
            }

            SignOut();

            if (!_userService.PasswordSignIn(user.UserName, userForLoginDto.Password, userForLoginDto.RememberMe))
            {
                return new ErrorDataResult<User>(user, "Email oder Password Invalid");
            }


            return new SuccessDataResult<User>(user);

        }




        [LogAspect(typeof(DatabaseLogger))]
        [ValidationAspect(typeof(UserForRegisterDtoValidation), Priority = 1)]
        [TransactionScopeAspect]
        [PerformanceAspect(5)]
        public IDataResult<User> Register(UserForRegisterDto userForRegisterDto)
        {

            User user = new User();

            var result = _userService.AddUser(userForRegisterDto, userForRegisterDto.Password);

            if (result.Success)
            {
                user = _userService.FindByEmail(userForRegisterDto.Email);

                string confirmToken = _userService.GenerateEmailConfirmationToken(user);
                var callBackUrl = CreateConfirmationCode(user.UserName, confirmToken, "ConfirmEmail");

                _mailService.SendConfirmMail(callBackUrl, new string[] { user.Email });

                return new SuccessDataResult<User>(user, Messages.UserRegistered);
            }

            return new ErrorDataResult<User>(user, result.Message);
        }

        [LogAspect(typeof(DatabaseLogger))]
        [ValidationAspect(typeof(UserForResetPasswordDtoValidator), Priority = 1)]
        //[TransactionScopeAspect]
        public IResult ResetPassword(UserForResetPasswordDto userForResetPasswordDto)
        {
            var user = _userService.FindByName(userForResetPasswordDto.UserName);
            if (user == null)
            {
                return new ErrorResult("User not found");
            }

            var result = _userService.ResetPassword(user, userForResetPasswordDto.Code, userForResetPasswordDto.Password);

            if (!result.Success)
            {
                return new ErrorResult(result.Message);
            }

            return new SuccessResult();

        }


        [LogAspect(typeof(DatabaseLogger))]

        //[TransactionScopeAspect]
        public IResult ConfirmEmail(string userName, string code)
        {
            if (userName == null || code == null)
            {
                return new ErrorResult("userName oder code is null");
            }

            var user = _userService.FindByName(userName);
            if (user == null)
            {
                return new ErrorResult("User not found");
            }

            var result = _userService.ConfirmUser(user, code);

            if (!result.Success)
            {
                return new ErrorResult(result.Message);
            }

            return new SuccessResult();

        }

        [LogAspect(typeof(DatabaseLogger))]
        [ValidationAspect(typeof(UserForForgotPasswordDtoValidator), Priority = 1)]
        // [TransactionScopeAspect]
        public IResult SendPasswordResetMail(UserForForgotPasswordDto userForForgotPasswordDto)
        {
            var user = _userService.FindByEmail(userForForgotPasswordDto.Email);

            if (user == null)
            {
                return new ErrorResult("User not found");
            }

            var confirmationCode = _userService.GeneratePasswordResetToken(user);

            var callBackUrl = CreateConfirmationCode(user.UserName, confirmationCode, "ResetPassword");

            _mailService.SendResetPasswordMail(callBackUrl, new string[] { user.Email });

            return new SuccessResult();
        }

        public void SignOut()
        {
            _userService.SignOut();
        }

        public IResult UserExists(string userName)
        {
            var user = _userService.FindByName(userName);
            if (user == null)
            {
                return new ErrorResult("User not found");
            }

            return new SuccessResult();

        }

        public string CreateConfirmationCode(string userName, string code, string path)
        {
            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Scheme = "https";
            uriBuilder.Host = "localhost";
            uriBuilder.Path = "account/" + path + "";
            uriBuilder.Port = 34080;
            uriBuilder.Query = "userName=" + userName + "&code=" + code + "";
            Uri uri = uriBuilder.Uri;

            return uri.AbsoluteUri;
        }

        public AuthenticationProperties GetAuthenticationProperties(string redirectUrl, string provider)
        {
            return _userService.GetAuthenticationProperties(redirectUrl,provider);
        }

        public IResult LoginWithExternalAccount()
        {
            var result =_userService.LoginWithExternalAccount();
            return result;
        }
    }
}
