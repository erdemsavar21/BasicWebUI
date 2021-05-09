using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.ServiceAdapters.AspIdentity.Model;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Dtos;
using Microsoft.AspNetCore.Authentication;

namespace Business.Abstract
{
    public interface IUserService
    {
        //List<OperationClaim> GetClaims(User user);
        IResult AddUser(UserForRegisterDto user,string password);
        //User GetByMail(string email);
        List<User> GetUsers();
        User FindByName(string userName);
        User FindByEmail(string email);
        bool PasswordSignIn(string userName, string password,bool rememberMe);
        bool IsEmailConfirmed(User user);
        void SignOut();
        string GeneratePasswordResetToken(User user);
        IResult ResetPassword(User user, string code, string password);
        IResult ConfirmUser(User user,string code);
        string GenerateEmailConfirmationToken(User user);
        IResult ChangePassword(string userName, UserForChangePasswordDto userForChangePasswordDto);
        IResult UpdateUser(User user);
        List<Role> GetRoles();
        IResult RoleCreate(Role role);
        IResult RoleDelete(string roleId);
        List<string> GetUserRoles(string userId);
        IResult AddRolesToUser(List<Role> roles,string userId);
        AuthenticationProperties GetAuthenticationProperties(string redirectUrl,string provider);
        IResult LoginWithExternalAccount();
    }
}
