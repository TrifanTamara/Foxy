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
        private static Dictionary<Guid, LessonModel> currentItem = new Dictionary<Guid, LessonModel>();

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
            
            return View("LessonPage", model);
        }
    }
}
