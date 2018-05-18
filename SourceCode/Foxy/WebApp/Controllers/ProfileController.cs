using Data.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebApp.Filter;

namespace WebApp.Controllers
{
    [Route("[controller]")]
    [DefaultControllerFilter]
    [Authorize]
    public class ProfileController : Controller
    {
        private IUsersRepository _repository;
        private readonly HttpClient _httpClient; // declare a HttpClient

        public ProfileController(IUsersRepository repository, HttpClient httpClient)
        {
            _repository = repository;
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
