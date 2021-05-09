using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models.Member;
using WebUI.Models.UserModel;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebUI.Controllers
{
    [Authorize(Roles="Admin,Editor")]
    public class AdminController : Controller
    {

        private IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var users = _userService.GetUsers();

            var userListViewModel = new UserListViewModel { Users = users };
            return View(userListViewModel);
        }

        public IActionResult Users()
        {

            var users = _userService.GetUsers();

            var userListViewModel = new UserListViewModel { Users = users };
            return View(userListViewModel);

        }

        public IActionResult RoleCreate()
        {
            var result = _userService.GetRoles();

            RoleViewModel roleViewModel = new RoleViewModel { Roles = result };

            return View(roleViewModel);
        }

        [HttpPost]
        public IActionResult RoleCreate(RoleViewModel roleViewModel)
        {

            if (!ModelState.IsValid)
            {
                return View(roleViewModel);
            }

            TempData.Remove("message");
            var result = _userService.RoleCreate(roleViewModel.Role);
            if (result.Success)
            {
                TempData.Add("message", "Role Added");
                return RedirectToAction("RoleCreate");
            }

            ModelState.AddModelError("", result.Message);

            return View(roleViewModel);
        }


        public IActionResult RoleDelete(string roleId)
        {
            var result = _userService.RoleDelete(roleId);
            if (result.Success)
            {
                TempData.Add("message", "Role deleted");
                return RedirectToAction("RoleCreate");
            }

            ModelState.AddModelError("", result.Message);

            return RedirectToAction("RoleCreate");
        }

        public IActionResult RoleAssign(string userId)
        {
            List<Role> roles = _userService.GetRoles();
            List<string> userRoles = _userService.GetUserRoles(userId);
            if (userRoles != null)
            {
                foreach (var role in roles)
                {
                    if (userRoles.Contains(role.RoleName))
                    {
                        role.IsChecked = true;
                    }
                }
            }

            RoleAssignViewModel roleAssignViewModel = new RoleAssignViewModel { UserId = userId, Roles = roles };

            return View(roleAssignViewModel);
        }

        [HttpPost]
        public IActionResult RoleAssign(RoleAssignViewModel roleAssignViewModel)
        {
            var result = _userService.AddRolesToUser(roleAssignViewModel.Roles, roleAssignViewModel.UserId);
            if (result.Success)
            {
                TempData.Add("message", "Roles Update");
                return RedirectToAction("RoleAssign",new {userId = roleAssignViewModel.UserId });
            }

            ModelState.AddModelError("", result.Message);

            return View(roleAssignViewModel);
        }
    }
}
