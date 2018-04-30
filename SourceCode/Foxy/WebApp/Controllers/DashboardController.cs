using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.Filter;
using WebApp.Models;

namespace WebApp.Controllers
{
    [DefaultControllerFilter]
    public class DashboardController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        { 
            return View();
        }
    }
}
