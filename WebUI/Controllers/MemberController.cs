using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models.Member;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebUI.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {

        private IUserService _userService;

        public MemberController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var user = _userService.FindByName(User.Identity.Name);
            var model = new MemberViewModel { CurrentView = "Index" ,User=user};
            return View(model);
        }

        public IActionResult PasswordChange()
        {
            var model = new MemberViewModel { CurrentView = "PasswordChange" };
            return View(model);
        }

        [HttpPost]
        public IActionResult PasswordChange(UserForChangePasswordDto userForChangePasswordDto)
        {

            var model = new MemberViewModel { CurrentView = "PasswordChange", userForChangePasswordDto=userForChangePasswordDto };
            
            if (!ModelState.IsValid)
            {
                
                return View(model);
            }

           

            var result = _userService.ChangePassword(User.Identity.Name, userForChangePasswordDto);

            if (result.Success)
            {
                TempData.Add("message", "Password has changed");
                return View(model);
            }

            ModelState.AddModelError("", result.Message);

            
            return View(model);
        }

        public IActionResult UpdateUserInfo()
        {
            var model = new MemberViewModel { CurrentView = "UpdateUserInfo" };
            var user = _userService.FindByName(User.Identity.Name);
            model.User = user;
            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateUserInfo(User user,IFormFile userPicture)
        {
            var model = new MemberViewModel { CurrentView = "UpdateUserInfo" };
            model.User = user;
            if (!ModelState.IsValid)
            {

                return View(model);
            }

            if (userPicture!=null && userPicture.Length>0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(userPicture.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/UserPicture", fileName);
                using (var stream = new FileStream(path,FileMode.Create))
                {
                    userPicture.CopyTo(stream);
                    user.Picture = "/images/UserPicture/" + fileName;
                }
            }

            var result = _userService.UpdateUser(user);

            if (result.Success)
            {
                TempData.Add("message", "User has updated");
                return View(model);
            }

            ModelState.AddModelError("", result.Message);



            return View(model);
        }

        public void LogOut()
        {
            _userService.SignOut();
        }

       
    }
}
