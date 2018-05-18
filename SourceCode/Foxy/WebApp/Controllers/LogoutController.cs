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
using WebApp.Filter;
using WebApp.Security;

namespace WebApp.Controllers
{
    [Route("[controller]")]
    [DefaultControllerFilter]
    [Authorize]
    public class LogoutController : Controller
    {
        private readonly IUsersRepository _repository;
        //private JwToken _jwtToken;
        //private readonly HttpClient _httpClient; // declare a HttpClient

        public LogoutController(IUsersRepository repository)
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
