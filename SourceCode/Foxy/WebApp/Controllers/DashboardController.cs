using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Filter;
using WebApp.Models;
using Data.Domain.Entities.UserRelated;
using Data.Domain.Entities;
using Data.Domain.Entities.TemplateItems;

namespace WebApp.Controllers
{
    [DefaultControllerFilter]
    [Authorize]
    public class DashboardController : Controller
    {
        private IUsersRepository _userRepo;
        private IVocabularItemRepository _vocabularRepo;

        public DashboardController(IUsersRepository userRepo, IVocabularItemRepository vocabularRepo)
        {

        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            DashboardModel model = new DashboardModel();
            string email = HttpContext.User.Claims.First().Value;
            User user = await _userRepo.GetByEmail(email);

            model.SeedLevelNr = (await _vocabularRepo.GetItemsByGrandLevels(user.Id, GrandLevels.Seed)).Count;
            model.LeafLevelNr = (await _vocabularRepo.GetItemsByGrandLevels(user.Id, GrandLevels.Leaf)).Count;
            model.BloomLevelNr = (await _vocabularRepo.GetItemsByGrandLevels(user.Id, GrandLevels.Bloom)).Count;
            model.FlourishedLevelNr = (await _vocabularRepo.GetItemsByGrandLevels(user.Id, GrandLevels.Flourished)).Count;

            model.RadicalsLesson = (await _vocabularRepo.GetVocabLessonByTypes(user.Id, user.Level, VocabularType.Radical, InfoLessonType.ActiveLesson));
            model.RadicalsViewed = (await _vocabularRepo.GetVocabLessonByTypes(user.Id, user.Level, VocabularType.Radical, InfoLessonType.ViewedLesson));
            model.RadicalsPassed = (await _vocabularRepo.GetVocabLessonByTypes(user.Id, user.Level, VocabularType.Radical, InfoLessonType.Passed));

            model.KanjisLesson = (await _vocabularRepo.GetVocabLessonByTypes(user.Id, user.Level, VocabularType.Kanji, InfoLessonType.ActiveLesson));
            model.KanjisViewed = (await _vocabularRepo.GetVocabLessonByTypes(user.Id, user.Level, VocabularType.Kanji, InfoLessonType.ViewedLesson));
            model.KanjisPassed = (await _vocabularRepo.GetVocabLessonByTypes(user.Id, user.Level, VocabularType.Kanji, InfoLessonType.Passed));

            model.Words = (await _vocabularRepo.GetVocabLessonByTypes(user.Id, user.Level, VocabularType.Word, InfoLessonType.ActiveLesson));

            return View();
        }
    }
}
