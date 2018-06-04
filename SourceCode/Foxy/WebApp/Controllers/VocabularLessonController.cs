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
    public class VocabularLessonController : Controller
    {
        private IUsersRepository _userRepo;
        private IVocabularItemRepository _vocabularRepo;
        private IMainService _service;
        private static Dictionary<Guid, LessonModel> currentSeesion = new Dictionary<Guid, LessonModel>();

        public VocabularLessonController(IUsersRepository userRepo, IVocabularItemRepository vocabularRepo, IMainService mainServ)
        {
            _userRepo = userRepo;
            _vocabularRepo = vocabularRepo;
            _service = mainServ;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string email = HttpContext.User.Claims.First().Value;
            User user = await _userRepo.GetByEmail(email);

            LessonModel model = new LessonModel();
            //set model list and instantiate list in model to avoid nullreferenec
            model.LessonList = await _vocabularRepo.GetItemsForLesson(user.UserId);

            return View(model);
        }

        [HttpGet]
        [Route("LessonPage")]
        public async Task<IActionResult> LessonPage([FromRoute]string name)
        {
            string email = HttpContext.User.Claims.First().Value;
            User user = await _userRepo.GetByEmail(email);

            LessonModel model = new LessonModel();
            //set model list and instantiate
            model.LessonList = await _vocabularRepo.GetItemsForLesson(user.UserId);
            model.ItemVisited = new List<bool>();
            foreach (var x in model.LessonList) model.ItemVisited.Add(false);
            model.CurrentIndex = 0;
            model.ItemVisited[0] = true;
            model.ReviewActive = false;
            currentSeesion[user.UserId] = model;
            return View("LessonPage", model);
        }
        
        [HttpGet]
        [Route("LessonReview")]
        public void LessonReview()
        {
            string email = HttpContext.User.Claims.First().Value;
            User user = _userRepo.GetByEmail(email).Result;

            LessonModel model = currentSeesion[user.UserId];
            _service.StartReviewSession(user.UserId, true, model.LessonList);

            RedirectToAction("ReviewLesson", "VocabularReview");
        }

    }
}
