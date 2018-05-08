﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using WebApp.DTOs;
using WebApp.Filter;
using WebApp.DTOs_Validators;

namespace WebApp.Controllers
{
    [Route("[controller]")]
    [DefaultControllerFilter]
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        private readonly IUsersRepository _userRepo;
        private readonly IVocabularItemRepository _vocabRepo;

        public RegisterController(IUsersRepository userRepo, IVocabularItemRepository vocabRepo)
        {
            _userRepo = userRepo;
            _vocabRepo = vocabRepo;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Index(Register dto)
        {
            if (ModelState.IsValid)
            {
                if (_userRepo.GetByEmail(dto.Email).Result != null)
                {
                    SharedInfo.RegisterError = "Email already exists!";
                    return Redirect("Register");
                }

                // Encrypt the password using SHA256
                byte[] bytes = Encoding.UTF8.GetBytes(dto.Password);
                SHA256Managed cipher = new SHA256Managed();
                byte[] hash = cipher.ComputeHash(bytes);
                string hashStr = "";
                foreach (byte b in hash)
                    hashStr += string.Format("{0:x2}", b);

                // Create the user and add it to database
                User user = Data.Domain.Entities.User.Create(dto.UserName, false, dto.Email, hashStr, null, "");
                await _userRepo.Add(user);
                await _vocabRepo.AddVocabularForNewUser(user.Id);
                // Redirect to login page with parameter registered
                SharedInfo.RegisterError = "";

                //Success
                SharedInfo.ShowSuccessMessage = "You are now registered!";
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Login"}));
            }
            //return View("index", dto);

            return View(dto);
        }
    }
}
