using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Concrete
{
    public class Role : IEntity
    {
        public Role()
        {
        }

        public string RoleId { get; set; }

        [Display(Name = "Role Name")]
        [Required(ErrorMessage = "Please enter Role Name")]
        public string RoleName { get; set; }
        public bool IsChecked { get; set; } = false;
    }
}
