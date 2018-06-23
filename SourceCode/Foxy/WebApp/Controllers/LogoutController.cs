using Data.Domain.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebApp.DTOs;

namespace WebApp.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class LogoutController : Controller
    {
        private readonly IUserRepo _repository;
        //private JwToken _jwtToken;

        public LogoutController(IUserRepo repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home" }));
        }
    }
}
