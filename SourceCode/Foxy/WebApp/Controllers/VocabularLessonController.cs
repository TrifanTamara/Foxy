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

namespace WebApp.Controllers
{
    [DefaultControllerFilter]
    [Authorize]
    [Route("[controller]")]
    public class VocabularLessonController : Controller
    {
        private IUsersRepository _userRepo;
        private IVocabularItemRepository _vocabularRepo;
        private static Dictionary<Guid, LessonModel> currentSeesion = new Dictionary<Guid, LessonModel>();

        public VocabularLessonController(IUsersRepository userRepo, IVocabularItemRepository vocabularRepo)
        {
            _userRepo = userRepo;
            _vocabularRepo = vocabularRepo;
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
        [Route("NextItem")]
        public JsonResult NextItem()
        {
            string email = HttpContext.User.Claims.First().Value;
            User user = _userRepo.GetByEmail(email).Result;

            LessonModel model = currentSeesion[user.UserId];
            bool showModal = false;
            if (model.CurrentIndex < model.LessonList.Count - 1) model.CurrentIndex++;
            if (model.CurrentIndex == model.LessonList.Count - 1 && model.ItemVisited[model.CurrentIndex] == false)
            {
                showModal = true;
                model.ReviewActive = true;
            }

            VocabularWrapper item = model.LessonList[model.CurrentIndex];
            return Json(new
            {
                name = item.Name,
                meaning = item.MainMeaning,
                activeIndex = model.CurrentIndex,
                activeReview = model.ReviewActive,
                showModal = showModal
            });
        }

        [HttpGet]
        [Route("PreviousItem")]
        public JsonResult PreviousItem()
        {
            string email = HttpContext.User.Claims.First().Value;
            User user = _userRepo.GetByEmail(email).Result;

            LessonModel model = currentSeesion[user.UserId];
            if (model.CurrentIndex > 0) model.CurrentIndex--;

            VocabularWrapper item = model.LessonList[model.CurrentIndex];
            return Json(new
            {
                name = item.Name,
                meaning = item.MainMeaning,
                activeIndex = model.CurrentIndex,
                activeReview = model.ReviewActive
            });
        }

        [HttpGet]
        [Route("ReadingTab")]
        public IActionResult ReadingTab()
        {
            string email = HttpContext.User.Claims.First().Value;
            User user = _userRepo.GetByEmail(email).Result;

            LessonModel model = currentSeesion[user.UserId];
            return PartialView("PartialView/ReadingTab", model.LessonList[model.CurrentIndex]);
        }

        [HttpGet]
        [Route("StructureTab")]
        public IActionResult StructureTab()
        {
            string email = HttpContext.User.Claims.First().Value;
            User user = _userRepo.GetByEmail(email).Result;

            LessonModel model = currentSeesion[user.UserId];
            return PartialView("PartialView/StructureTab", model.LessonList[model.CurrentIndex]);
        }

        [HttpGet]
        [Route("MeaningTab")]
        public IActionResult MeaningTab()
        {
            string email = HttpContext.User.Claims.First().Value;
            User user = _userRepo.GetByEmail(email).Result;

            LessonModel model = currentSeesion[user.UserId];

            return PartialView("PartialView/MeaningTab", model.LessonList[model.CurrentIndex]);
        }
    }
}
