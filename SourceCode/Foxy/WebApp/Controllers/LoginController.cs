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
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace WebApp.Controllers
{
    [Route("[controller]")]
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly IUserRepo _repository;

        public LoginController(IUserRepo repository)
        {
            _repository = repository;

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hosting.json", true)
                .AddJsonFile("appsettings.json", true)
                .Build();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginDto dto)
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
                        //var token = _jwtToken.GenerateToken(user);


                        // create claims
                        List<Claim> claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.Name, user.Username)
                        };

                        // create identity
                        ClaimsIdentity identity = new ClaimsIdentity(claims, "login");

                        // create principal
                        ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                        // sign-in
                        await HttpContext.SignInAsync(principal);

                        //LoggedUser.IsLogged = true;
                        //LoggedUser.Email = user.Email;
                        //LoggedUser.UserName = user.Username;

                        SharedInfo.LoginError = "";
                        SharedInfo.ShowSuccessMessage = "You are now logged in!";
                        return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Dashboard" }));
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