using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Business.ServiceAdapters.AspIdentity.Model;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebUI.Helpers;
using WebUI.Models;
using WebUI.Models.Account;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebUI.Controllers
{

    public class AccountController : Controller
    {
        private IAuthService _authService;

        

        public AccountController(IAuthService authService, SignInManager<AppIdentityUser> signInManager)
        {
            _authService = authService;
            
        }

        // GET: /<controller>/
        public IActionResult Register()
        {


            return View();
        }

        [HttpPost]
        public IActionResult Register(UserForRegisterDto userForRegisterDto)
        {

            if (!ModelState.IsValid)
            {
                return View(new AccountViewModel { userForRegisterDto = userForRegisterDto });
            }

           

            var result = _authService.Register(userForRegisterDto);
            if (result.Success)
            {
                TempData.Add("message", "Please confirm your Email...");
                
                return RedirectToAction("Login", "Account");
            }

            
            ModelState.AddModelError("", result.Message);



            return View(new AccountViewModel { userForRegisterDto = userForRegisterDto });
        }

        public IActionResult ConfirmEmail(string userName, string code)
        {
            if (userName == null || code == null)
            {
                return RedirectToAction("Index", "Home");
            }

            code = code.Replace(" ", "+");

            var result = _authService.ConfirmEmail(userName, code);

            if (!result.Success)
            {
                throw new ApplicationException(result.Message);

            }



            return RedirectToAction("Login", "Account");


        }

        public IActionResult Login(string returnUrl)
        {
            if (!String.IsNullOrEmpty(returnUrl))
            {
                TempData["ReturnUrl"] = returnUrl;
            }


            return View();
        }

        [HttpPost]
        public IActionResult Login(UserForLoginDto userForLoginDto)
        {

            if (!ModelState.IsValid)
            {
                return View(new AccountViewModel { userForLoginDto = userForLoginDto });
            }

            var result = _authService.Login(userForLoginDto);

            if (result.Success)
            {
                // TempData.Add("message", "Giris Yapildi");
                // AlertMessageHelper.Success(this, "Giris yapildi", true);
                
                if (TempData["ReturnUrl"] != null)
                {
                    return Redirect(TempData["ReturnUrl"].ToString());
                }
                return RedirectToAction("index", "Home");
            }

            ModelState.AddModelError("", result.Message);

            return View(new AccountViewModel { userForLoginDto = userForLoginDto });
        }

        public IActionResult ForgotPassword()
        {

            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(UserForForgotPasswordDto userForForgotPasswordDto)
        {

            if (!ModelState.IsValid)
            {
                return View(new AccountViewModel { userForForgotPasswordDto = userForForgotPasswordDto });
            }


           var result = _authService.SendPasswordResetMail(userForForgotPasswordDto);

            if (result.Success)
            {
                TempData.Add("message", "E-Mail ist gesendet");
                
                return RedirectToAction("ForgotPasswordEmailSent", "Account");
            }

            ModelState.AddModelError("", result.Message);

            return View(new AccountViewModel { userForForgotPasswordDto = userForForgotPasswordDto });
        }

        public IActionResult ForgotPasswordEmailSent()
        {
            return View();
        }

        public IActionResult ResetPassword(string userName, string code)
        {
            if (userName == null || code == null)
            {
                return RedirectToAction("Index", "Home");
            }
            
            code = code.Replace(" ", "+");

            var result = _authService.UserExists(userName);

            if (!result.Success)
            {
                throw new ApplicationException(result.Message);
            }
            UserForResetPasswordDto userForResetPasswordDto = new UserForResetPasswordDto { Code = code,UserName = userName };
            var model = new AccountViewModel { userForResetPasswordDto = userForResetPasswordDto };
            return View(model);

        }

        [HttpPost]
        public IActionResult ResetPassword(UserForResetPasswordDto userForResetPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return View(userForResetPasswordDto);
            }

            var result = _authService.ResetPassword(userForResetPasswordDto);

            if (result.Success)
            {
                return RedirectToAction("ResetPasswordConfirm");
            }

            ModelState.AddModelError("", result.Message);

            return View();

        }

        public IActionResult ResetPasswordConfirm()
        {
            return View();
        }

     
        public IActionResult AccessDenied()
        {
            return View();
        }


        public IActionResult LoginFacebook(string returnUrl)
        {
            string redirectUrl = Url.Action("ExternalResponse", "Account", new { ReturnUrl = returnUrl });

            var properties = _authService.GetAuthenticationProperties(redirectUrl,"Facebook");

            return new ChallengeResult("Facebook", properties);
        }

        public IActionResult LoginGoogle(string returnUrl)
        {
            string redirectUrl = Url.Action("ExternalResponse", "Account", new { ReturnUrl = returnUrl });

            var properties = _authService.GetAuthenticationProperties(redirectUrl,"Google");

            return new ChallengeResult("Google", properties);
        }

        public IActionResult ExternalResponse(string returnUrl="/")
        {
            var result = _authService.LoginWithExternalAccount();

            if (result.Success)
            {
                return Redirect(returnUrl);
            }

            TempData.Add("message", result.Message);

            return RedirectToAction("Login");
        }


    }
}
