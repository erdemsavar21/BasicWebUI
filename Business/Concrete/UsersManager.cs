using System;
using System.Collections.Generic;
using System.Linq;
using Business.Abstract;
using Business.ServiceAdapters.AspIdentity.Model;
using Core.Aspects.Autofac.Caching;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Dtos;
using Microsoft.AspNetCore.Identity;
using Mapster;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Aspects.Autofac.Transaction;
using System.Reflection;
using System.Threading;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Business.Concrete
{
    public class UsersManager : IUserService
    {
        private UserManager<AppIdentityUser> _userManager;
        private SignInManager<AppIdentityUser> _signInManager;
        private RoleManager<AppIdentityRole> _roleManager;
        public UsersManager(UserManager<AppIdentityUser> usermanager, SignInManager<AppIdentityUser> signInManager, RoleManager<AppIdentityRole> roleManager)
        {
            _userManager = usermanager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IResult AddUser(UserForRegisterDto user, string password)
        {
            var appUser = new AppIdentityUser
            {
                UserName = user.Email.ToLower(),
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                City = user.City,
                Adresse = user.Adresse,
                Gender = user.Gender,
                Picture = user.Picture,
                Birthday = user.Birthday
            };
            var result = _userManager.CreateAsync(appUser, password).Result;

            if (result.Succeeded)
            {
                return new SuccessResult();
            }



            return new ErrorResult(result.Errors.FirstOrDefault().Description);
        }

        [CacheAspect(duration: 5)]
        public List<User> GetUsers()
        {
            List<User> users = new List<User>();
            var userList = _userManager.Users.ToList();

            foreach (var user in userList)
            {
                users.Add(ConvertIdentityUser(user));
            }

            return users;
        }

        public User FindByName(string userName)
        {
            var user = _userManager.FindByNameAsync(userName).Result;
            if (user == null) return null;

            return ConvertIdentityUser(user);
        }

        public User FindByEmail(string email)
        {
            var user = _userManager.FindByEmailAsync(email).Result;
            if (user == null)
            {
                return null;
            }
            return ConvertIdentityUser(user);
        }

        public AppIdentityUser FindIdentityUserByEmail(string email)
        {
            return _userManager.FindByEmailAsync(email).Result;
        }

        public AppIdentityUser FindIdentityUserByUserName(string userName)
        {
            return _userManager.FindByNameAsync(userName).Result;
        }

        public bool PasswordSignIn(string userName, string password, bool rememberMe)
        {
            return _signInManager.PasswordSignInAsync(userName, password, rememberMe, false).Result.Succeeded;

        }

        public bool IsEmailConfirmed(User user)
        {
            return _userManager.IsEmailConfirmedAsync(_userManager.FindByNameAsync(user.UserName).Result).Result;
        }



        public void SignOut()
        {
            _signInManager.SignOutAsync().Wait();
        }

        public string GeneratePasswordResetToken(User user)
        {
            var confirmationCode = _userManager.GeneratePasswordResetTokenAsync(FindIdentityUserByEmail(user.Email)).Result;

            return confirmationCode;
        }

        public IResult ResetPassword(User user, string code, string password)
        {
            var identityUser = FindIdentityUserByEmail(user.Email);
            var result = _userManager.ResetPasswordAsync(identityUser, code, password).Result;
            if (!result.Succeeded)
            {
                return new ErrorResult(result.Errors.FirstOrDefault().Description);
            }

            _userManager.UpdateSecurityStampAsync(identityUser); //Yeniden login olmasini saglamak icin tüm sistemlerde default kontrol süresi 30 dk

            return new SuccessResult();
        }

        public IResult ConfirmUser(User user, string code)
        {
            var identityUser = FindIdentityUserByEmail(user.Email);
            var result = _userManager.ConfirmEmailAsync(identityUser, code).Result;
            if (!result.Succeeded)
            {
                return new ErrorResult(result.Errors.FirstOrDefault().Description);
            }

            return new SuccessResult();
        }

        public string GenerateEmailConfirmationToken(User user)
        {
            string confirmationCode = _userManager.GenerateEmailConfirmationTokenAsync(FindIdentityUserByEmail(user.Email)).Result;

            return confirmationCode;
        }


        [ValidationAspect(typeof(UserForChangePasswordDtoValidator), Priority = 1)]

        [LogAspect(typeof(DatabaseLogger))]
        public IResult ChangePassword(string userName, UserForChangePasswordDto userForChangePasswordDto)
        {

            var identityUser = FindIdentityUserByUserName(userName);
            bool checkPassword = _userManager.CheckPasswordAsync(identityUser, userForChangePasswordDto.PasswordOld).Result;
            if (!checkPassword)
            {
                return new ErrorResult("Password is not correct");
            }
            var result = _userManager.ChangePasswordAsync(identityUser, userForChangePasswordDto.PasswordOld, userForChangePasswordDto.PasswordNew).Result;
            if (result.Succeeded)
            {
                _userManager.UpdateSecurityStampAsync(identityUser).Wait();
                _signInManager.SignOutAsync().Wait();
                _signInManager.PasswordSignInAsync(identityUser, userForChangePasswordDto.PasswordNew, true, false).Wait();

                return new SuccessResult();
            }

            return new ErrorResult(result.Errors.FirstOrDefault().Description);
        }


        [ValidationAspect(typeof(UserForUpdateUserInfoValidator), Priority = 1)]
        [LogAspect(typeof(DatabaseLogger))]
        public IResult UpdateUser(User user)
        {
            var identityUser = ConvertUserToIdentityUser(user);
            var result = _userManager.UpdateAsync(identityUser).Result;
            if (result.Succeeded)
            {

                _userManager.UpdateSecurityStampAsync(identityUser).Wait();
                _signInManager.SignOutAsync().Wait();

                _signInManager.SignInAsync(identityUser, true).Wait();
                return new SuccessResult();
            }

            return new ErrorResult(result.Errors.FirstOrDefault().Description);
        }


        public User ConvertIdentityUser(AppIdentityUser appIdentityUser)
        {
            //return new User { Id = appIdentityUser.Id,UserName=appIdentityUser.UserName, Email = appIdentityUser.Email, PhoneNumber = appIdentityUser.PhoneNumber };
            return appIdentityUser.Adapt<User>();
        }

        public AppIdentityUser ConvertUserToIdentityUser(User user)
        {
            //return new User { Id = appIdentityUser.Id,UserName=appIdentityUser.UserName, Email = appIdentityUser.Email, PhoneNumber = appIdentityUser.PhoneNumber };
            AppIdentityUser appIdentityUser = _userManager.FindByEmailAsync(user.Email).Result;
            appIdentityUser.UserName = user.UserName;
            appIdentityUser.PhoneNumber = user.PhoneNumber;
            appIdentityUser.Birthday = user.Birthday;
            appIdentityUser.Gender = user.Gender;
            appIdentityUser.Picture = user.Picture;
            appIdentityUser.City = user.City;
            appIdentityUser.Adresse = user.Adresse;

            //AppIdentityUser adpUser = user.Adapt<AppIdentityUser>();

            //foreach (PropertyInfo propertyIdentity in appIdentityUser.GetType().GetProperties())
            //{
            //    foreach (PropertyInfo propertyUser in appIdentityUser.GetType().GetProperties())
            //    {
            //        if (propertyIdentity.Name ==propertyUser.Name)
            //        {

            //        }
            //    }
            //}

            return appIdentityUser;
        }

        public List<Role> GetRoles()
        {
            List<Role> roles = new List<Role>();
            var result = _roleManager.Roles.ToList();

            foreach (var roleItem in result)
            {
                Role role = new Role { RoleId = roleItem.Id, RoleName = roleItem.Name };
                roles.Add(role);
            }

            return roles;

        }

        public IResult RoleCreate(Role role)
        {
            AppIdentityRole appIdentityRole = new AppIdentityRole();
            appIdentityRole.Name = role.RoleName;
            var result = _roleManager.CreateAsync(appIdentityRole).Result;
            if (!result.Succeeded)
            {
                return new ErrorResult(result.Errors.FirstOrDefault().Description);
            }

            return new SuccessResult();
        }

        public IResult RoleDelete(string roleId)
        {
            AppIdentityRole appIdentityRole = _roleManager.FindByIdAsync(roleId).Result;
            if (appIdentityRole != null)
            {
                var result = _roleManager.DeleteAsync(appIdentityRole).Result;
                if (result.Succeeded)
                {
                    return new SuccessResult();
                }

                return new ErrorResult(result.Errors.FirstOrDefault().Description);
            }

            return new ErrorResult("Role not found");
        }

        public List<string> GetUserRoles(string userId)
        {
            AppIdentityUser appIdentityUser = _userManager.FindByIdAsync(userId).Result;
            if (appIdentityUser != null)
            {
                return _userManager.GetRolesAsync(appIdentityUser).Result.ToList();
            }

            return null;
        }

        public IResult AddRolesToUser(List<Role> roles, string userId)
        {
            AppIdentityUser appIdentityUser = _userManager.FindByIdAsync(userId).Result;
            if (appIdentityUser != null)
            {
                var userRoles = _userManager.GetRolesAsync(appIdentityUser).Result;
                if (userRoles != null && userRoles.Count > 0)
                {
                    _userManager.RemoveFromRolesAsync(appIdentityUser, userRoles.ToArray()).Wait();

                }

                foreach (var role in roles)
                {
                    if (role.IsChecked)
                    {
                        _userManager.AddToRoleAsync(appIdentityUser, role.RoleName).Wait();

                    }
                }

                return new SuccessResult();
            }

            return new ErrorResult("User not found");
        }

        public AuthenticationProperties GetAuthenticationProperties(string redirectUrl,string provider)
        {
            var result = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", redirectUrl);
            return result;
        }

        public IResult LoginWithExternalAccount()
        {
            ExternalLoginInfo info = _signInManager.GetExternalLoginInfoAsync().Result;

            if (info==null)
            {
                return new ErrorResult("Login Failed");

            }

            var identityUser = _userManager.FindByEmailAsync(info.Principal.FindFirst(ClaimTypes.Email).Value).Result;

            if (identityUser!=null)
            {
               
                _signInManager.SignInAsync(identityUser, true).Wait();
                return new SuccessResult();
            }
           

            _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true).Wait();
           

            AppIdentityUser user = new AppIdentityUser();

            user.Email = info.Principal.FindFirst(ClaimTypes.Email).Value;
            user.UserName= info.Principal.FindFirst(ClaimTypes.Email).Value.ToString().ToLower();
            string facebookUserId = info.Principal.FindFirst(ClaimTypes.NameIdentifier).Value;
            user.Id = facebookUserId;
            var userResult = _userManager.CreateAsync(user).Result;
            if (!userResult.Succeeded)
            {
                return new ErrorResult(userResult.Errors.FirstOrDefault().Description);
            }

            _userManager.AddLoginAsync(user, info).Wait();

            _signInManager.SignInAsync(user, true).Wait();

            return new SuccessResult();


        }
    }
}