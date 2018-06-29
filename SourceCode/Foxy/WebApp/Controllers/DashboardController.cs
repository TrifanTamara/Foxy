using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using Data.Domain.Entities.UserRelated;
using Data.Domain.Entities;
using Data.Domain.Entities.TemplateItems;
using Data.Domain.Interfaces.UserRelated;
using Data.Domain.Wrappers;
using WebApplication.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private IUserRepo _userRepo;
        private IVocabularItemRepo _vocabularRepo;
        private IFormularItemRepo _formularRepo;

        public DashboardController(IUserRepo userRepo, IVocabularItemRepo vocabularRepo, IFormularItemRepo formular)
        {
            _userRepo = userRepo;
            _vocabularRepo = vocabularRepo;
            _formularRepo = formular;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            DashboardModel model = new DashboardModel();
            string email = HttpContext.User.Claims.First().Value;
            User user = await _userRepo.GetByEmail(email);

            model.SeedLevelNr = (await _vocabularRepo.GetItemsByGrandLevels(user.UserId, GrandLevels.Seed)).Count;
            model.LeafLevelNr = (await _vocabularRepo.GetItemsByGrandLevels(user.UserId, GrandLevels.Leaf)).Count;
            model.BloomLevelNr = (await _vocabularRepo.GetItemsByGrandLevels(user.UserId, GrandLevels.Bloom)).Count;
            model.FlourishedLevelNr = (await _vocabularRepo.GetItemsByGrandLevels(user.UserId, GrandLevels.Flourished)).Count;

            model.RadicalsLesson = (await _vocabularRepo.GetVocabLessonByTypes(user.UserId, user.Level, VocabularType.Radical, InfoLessonType.ActiveLesson));
            model.RadicalsViewed = (await _vocabularRepo.GetVocabLessonByTypes(user.UserId, user.Level, VocabularType.Radical, InfoLessonType.ViewedLesson));
            model.RadicalsPassed = (await _vocabularRepo.GetVocabLessonByTypes(user.UserId, user.Level, VocabularType.Radical, InfoLessonType.Passed));

            model.KanjisLesson = (await _vocabularRepo.GetVocabLessonByTypes(user.UserId, user.Level, VocabularType.Kanji, InfoLessonType.ActiveLesson));
            model.KanjisViewed = (await _vocabularRepo.GetVocabLessonByTypes(user.UserId, user.Level, VocabularType.Kanji, InfoLessonType.ViewedLesson));
            model.KanjisPassed = (await _vocabularRepo.GetVocabLessonByTypes(user.UserId, user.Level, VocabularType.Kanji, InfoLessonType.Passed));

            model.WordsLesson = (await _vocabularRepo.GetVocabLessonByTypes(user.UserId, user.Level, VocabularType.Word, InfoLessonType.ActiveLesson));
            model.WordsPassed = (await _vocabularRepo.GetVocabLessonByTypes(user.UserId, user.Level, VocabularType.Word, InfoLessonType.Passed));
            model.WordsTotal = (await _vocabularRepo.GetAllVocabByItemType(user.UserId, VocabularType.Word));

            model.CalculatePercentages();
            
            model.Grammar = new GrammarModel(await _formularRepo.GetAllFormByUserAndType(user.UserId, Data.Domain.Entities.TemplateItems.FormType.Grammar));


            List<FormularWrapper> readL = await _formularRepo.GetAllFormByUserAndType(user.UserId, Data.Domain.Entities.TemplateItems.FormType.Reading);
            List<FormularWrapper> listL = await _formularRepo.GetAllFormByUserAndType(user.UserId, Data.Domain.Entities.TemplateItems.FormType.Listening);

            model.Reading = new ReadListModel(readL, listL);
            

            return View(model);
        }
    }
}
