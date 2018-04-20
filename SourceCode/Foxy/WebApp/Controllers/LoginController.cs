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

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [DefaultControllerFilter]
    public class LoginController : Controller
    {
        private readonly IUsersRepository _repository;
        private JwToken _jwtToken;
        public LoginController(IUsersRepository repository)
        {
            _repository = repository;

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hosting.json", true)
                .AddJsonFile("appsettings.json", true)
                .Build();
            _jwtToken = new JwToken(config);
        }

//        [HttpGet]
//        public IActionResult Login()
//        {
//            // Get the Registered parameter if available
//            HttpContext.Request.Query.TryGetValue("Registered", out var registered);
//
//            // Send registered value to the view
//            if (registered.Equals(StringValues.Empty))
//                ViewData["Registered"] = 0;
//            else
//                ViewData["Registered"] = int.Parse(registered);
//            return View();
//        }

        [HttpPost]
        public async Task<IActionResult> Login(Login dto)
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

                   // HttpContext.Session.SetString("user_id", user.Id.ToString());
                    // Redirect to homepage
                    return Ok(token);
                }
                ModelState.AddModelError("", "Invalid password!");
            }
            else
                ModelState.AddModelError("", "Account is not registered!");

            // If something went bad, return the model back to view
            //return View(dto);
            return Unauthorized();
        }
    }
}