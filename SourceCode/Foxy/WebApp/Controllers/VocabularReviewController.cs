using Business.Wrappers;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.DTOs;
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

        [HttpPost]
        [Route("CheckAnswer")]
        public async Task<JsonResult> CheckAnswer(VocabularAnswerDto answer)
        {
            if (answer != null && answer.Meaning != null)
            {
                string email = HttpContext.User.Claims.First().Value;
                User user = await _userRepo.GetByEmail(email);

                AnswerStatusModel status = _service.UserAnswered(user.UserId, answer);
                return Json(new
                {
                    status.Reading,
                    status.Meaning,
                    status.Final,
                    status.LevelName
                });
            }
            return null;
        }

        [HttpGet]
        [Route("NextReview")]
        public async Task<JsonResult> NextReview()
        {
            string email = HttpContext.User.Claims.First().Value;
            User user = await _userRepo.GetByEmail(email);

            VocabularWrapper item = _service.GetItemForReview(user.UserId);
            return Json(new
            {
                Name = item.Name,
                Type = item.VocabularType
            });

        }

        [HttpGet]
        [Route("CurrentItem")]
        public IActionResult ReadingTab()
        {
            string email = HttpContext.User.Claims.First().Value;
            User user = _userRepo.GetByEmail(email).Result;

            VocabularWrapper item = _service.GetCurrentItem(user.UserId);
            return PartialView("PartialView/ReviewInfo", item);
        }
    }
}
