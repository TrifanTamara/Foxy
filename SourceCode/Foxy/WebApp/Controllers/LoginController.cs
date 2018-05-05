using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using WebApp.DTOs;
using WebApp.Filter;
using WebApp.Security;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using Microsoft.AspNetCore.Routing;

namespace WebApp.Controllers
{
    [Route("[controller]")]
    [DefaultControllerFilter]
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly IUsersRepository _repository;
        private JwToken _jwtToken;
        private readonly HttpClient _httpClient; // declare a HttpClient

        public LoginController(IUsersRepository repository, HttpClient httpClient)
        {
            _repository = repository;

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hosting.json", true)
                .AddJsonFile("appsettings.json", true)
                .Build();
            _jwtToken = new JwToken(config);
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Login dto)
        {
            if (ModelState.IsValid)
            {
                User user = await _repository.GetByEmail(dto.Email);

                // Check if the user exists
                if (user != null)
                {
                    // Hash the model password
                    byte[] bytes = Encoding.UTF8.GetBytes(dto.Password);
                    SHA256Managed cipher = new SHA256Managed();
                    byte[] hash = cipher.ComputeHash(bytes);
                    string hashStr = "";
                    foreach (byte b in hash)
                        hashStr += string.Format("{0:x2}", b);

                    // Check if the passwords match
                    if (user.Password.Equals(hashStr))
                    {
                        // Generate the token
                        var token = _jwtToken.GenerateToken(user);
                        //_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                        //HttpContext.Request.Headers["Authorization"] = "Bearer "+token;
                        //_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                        //return Ok(token);


                        LoggedUser.IsLogged = true;
                        LoggedUser.Email = user.Email;
                        LoggedUser.UserName = user.Username;

                        SharedInfo.LoginError = "";
                        return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Dashboard"}));
                    }
                }
                SharedInfo.LoginError = "Invalid email or password!";
                return Redirect("Login");
            }
            // If something went bad, return the model back to view
            return View(dto);
        }
    }
}