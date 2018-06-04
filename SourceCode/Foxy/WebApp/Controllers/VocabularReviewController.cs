using Business.Wrappers;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Filter;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    [DefaultControllerFilter]
    [Authorize]
    [Route("[controller]")]
    public class VocabularReviewController : Controller
    {
        private IUsersRepository _userRepo;
        private IVocabularItemRepository _vocabularRepo;
        private IMainService _service;

        public VocabularReviewController(IUsersRepository userRepo, IVocabularItemRepository vocabularRepo, IMainService mainSerice)
        {
            _userRepo = userRepo;
            _vocabularRepo = vocabularRepo;
            _service = mainSerice;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string email = HttpContext.User.Claims.First().Value;
            User user = await _userRepo.GetByEmail(email);

            _service.StartReviewSession(user.UserId, false);
            VocabularWrapper item = _service.GetItemForReview(user.UserId);

            return View("Review", item);
        }

        [HttpGet]
        [Route("ReviewLesson")]
        public async Task<IActionResult> ReviewLesson()
        {
            string email = HttpContext.User.Claims.First().Value;
            User user = await _userRepo.GetByEmail(email);

            VocabularWrapper item = _service.GetItemForReview(user.UserId);
            return View("Review", item);
        }


    }
}
