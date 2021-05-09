using System;
using System.Collections.Generic;
using Core.Entities.Concrete;

namespace WebUI.Models.UserModel
{
    public class UserListViewModel
    {
        public UserListViewModel()
        {
        }
        
        public List<User> Users { get; set; }
       
    }
}
