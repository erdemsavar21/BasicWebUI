using System;
using System.Collections.Generic;
using Core.Entities.Concrete;

namespace WebUI.Models.Member
{
    public class RoleViewModel
    {
        public RoleViewModel()
        {
        }

        public Role Role { get; set; }
        public List<Role> Roles { get; set; }
    }
}
