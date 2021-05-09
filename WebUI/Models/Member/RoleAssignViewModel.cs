using System;
using System.Collections.Generic;
using Core.Entities.Concrete;

namespace WebUI.Models.Member
{
    public class RoleAssignViewModel
    {
        public RoleAssignViewModel()
        {
        }

        public List<Role> Roles { get; set; }
        public string UserId { get; set; }
    }
}
