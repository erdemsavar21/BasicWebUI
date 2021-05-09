using System;
using Core.Entities.Concrete;
using Entities.Dtos;

namespace WebUI.Models.Member
{
    public class MemberViewModel
    {
        public MemberViewModel()
        {
        }

        public User User { get; set; }
        public string CurrentView { get; set; }
        public UserForChangePasswordDto userForChangePasswordDto { get; set; }
    }
}
