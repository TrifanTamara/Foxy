using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Data.Domain.Interfaces.UserRelated;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApp.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class ProfileController : Controller
    {
        private IUserRepo _userRepo;
        private IVocabularItemRepo _vocabularRepo;
        private IFormularItemRepo _formularRepo;

        public ProfileController(IUserRepo userRepo, IVocabularItemRepo vocabularRepo, IFormularItemRepo formular)
        {
            _userRepo = userRepo;
            _vocabularRepo = vocabularRepo;
            _formularRepo = formular;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string email = HttpContext.User.Claims.First().Value;
            User user = await _userRepo.GetByEmail(email);

            ProfileModel model = new ProfileModel();

            model.User = user;

            model.favoriteRadical = await _vocabularRepo.GetFavItems(user.UserId, Data.Domain.Entities.TemplateItems.VocabularType.Radical);
            model.favoriteKanji = await _vocabularRepo.GetFavItems(user.UserId, Data.Domain.Entities.TemplateItems.VocabularType.Kanji);
            model.favoriteWords = await _vocabularRepo.GetFavItems(user.UserId, Data.Domain.Entities.TemplateItems.VocabularType.Word);

            model.favoriteGrammar = (await _formularRepo.GetFavoriteFormsType(user.UserId, Data.Domain.Entities.TemplateItems.FormType.Grammar)).OrderBy(x=>x.PartialViewId).ToList();

            model.favoriteReading = (await _formularRepo.GetFavoriteFormsType(user.UserId, Data.Domain.Entities.TemplateItems.FormType.Reading)).OrderBy(x => x.PartialViewId).ToList();
            model.favoriteReading.AddRange((await _formularRepo.GetFavoriteFormsType(user.UserId, Data.Domain.Entities.TemplateItems.FormType.Listening)).OrderBy(x => x.PartialViewId).ToList());
            return View(model);
        }

        [HttpPost]
        [Route("setSession")]
        public async Task<JsonResult> setSession(int value)
        {
            string email = HttpContext.User.Claims.First().Value;
            User user = await _userRepo.GetByEmail(email);

            if(value<5 || value > 10)
            {
                return Json(new { success = false, length = user.LessonSize });
            }

            user.Update(user.Username, user.Email, user.Password, user.Description, user.Level, Guid.Empty, value);
            _userRepo.Edit(user);

            return Json(new { success = true, length = value });
        }
    }
}
