﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApp.Filter;
using WebApp.Models;

namespace WebApp.Controllers
{
    [DefaultControllerFilter]
    public class HomeController : Controller
    {
        [HttpGet]
        //[Route("index")]
        public IActionResult Index()
        {
            ViewBag.LoggedIn = false;

            return View();
        }

        [HttpGet]
        [Route("error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
