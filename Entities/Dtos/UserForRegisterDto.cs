using System;
using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace Entities.Dtos
{
    public class UserForRegisterDto:IDto
    {
        public UserForRegisterDto()
        {
        }

        //[Required(ErrorMessage = "Username is necessary")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email is not the correct format")]
        //[Required(ErrorMessage = "Email is necessary")]
        public string Email { get; set; }
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Password muss mindestens 4 Zeichen lang sein")]
        //[Required(ErrorMessage = "Password is necessary")]
        public string Password { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }
        [Display(Name = "Adresse")]
        public string Adresse { get; set; }
        [Display(Name = "Picture")]
        public string Picture { get; set; }
        [Display(Name = "Birthday")]
        public DateTime? Birthday { get; set; }
        [Display(Name = "Gender")]
        public int Gender { get; set; }

    }
}
