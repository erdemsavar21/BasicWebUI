using System;
using System.Linq;
using Business.Abstract;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebUI.TagHelpers
{
    [HtmlTargetElement("user-roles")]
    public class UserRoleTagHelper : TagHelper
    {

        private IUserService _userService;
        public UserRoleTagHelper(IUserService userService)
        {
            _userService = userService;
        }


        [HtmlAttributeName("user-id")]
        public string UserId { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            var roles = _userService.GetUserRoles(UserId);
            string html = string.Empty;
            roles.ToList().ForEach(p=> { html += $"<span class='badge badge-Info'> {p} </span> "; });
            output.Content.SetHtmlContent(html);
        }
    }
}
