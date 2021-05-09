using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace WebUI.ViewComponents
{
    public class MemberMenuViewComponent : ViewComponent
    {
        public MemberMenuViewComponent()
        {
        }

        public ViewViewComponentResult Invoke()
        {
            return View();
        }

    }
}
