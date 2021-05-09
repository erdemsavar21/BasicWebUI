using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebUI.Extensions;
using WebUI.Helpers;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        
    

        public HomeController()
        {
            
          
        }

        public IActionResult Index()
        {


            //  HttpContext.Session.SetObject("jwtToken", token.Data);   Json web token icin kullaniyoruz


            //AlertMessageHelper.Success(this, "Eklendi", true); 
           
            return View();
        }


      

    

  
     

        
    }
}
